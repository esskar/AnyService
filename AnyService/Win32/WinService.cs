using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;

using AnyService.Win32.API;

namespace AnyService.Win32
{
    public abstract partial class WinService : ServiceBase
    {        
        private string m_displayName = "";

        public WinService()
            : this(null)
        {
        }

        public WinService(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = this.GetType().Name;

            if (name.Length > ServiceBase.MaxNameLength)
                throw new ArgumentException("Length of name must not exceed " + ServiceBase.MaxNameLength, "name");

            this.ServiceName = name;
            this.CanStop = true;
            this.CanShutdown = true;
            this.CanPauseAndContinue = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanHandlePowerEvent = false;
        }        

        public string DisplayName
        {
            get { return string.IsNullOrEmpty(m_displayName) ? this.ServiceName : m_displayName; }
            set { m_displayName = value; }
        }

        public void Run()
        {
            ServiceBase.Run(this);
        }

        public void Start()
        {
            using (WinServiceNative native = new WinServiceNative())
            {
                native.Lock();
                native.Open(this.ServiceName);

                ApiStructs.ServiceStatus ss = native.QueryStatus();
                if (ss.CurrentState != ApiEnums.ServiceState.SERVICE_STOPPED)
                    throw new Win32Exception("Cannot start the service because it is already running");

                native.Start();
            }            
        }

        public new void Stop()
        {
            using (WinServiceNative native = new WinServiceNative())
            {
                native.Lock();
                native.Open(this.ServiceName);

                ApiStructs.ServiceStatus ss = native.QueryStatus();
                if (ss.CurrentState != ApiEnums.ServiceState.SERVICE_RUNNING)
                    throw new Win32Exception("Cannot start the service because it is not running");
            }
        }
        

        public void Uninstall()
        {

            using (WinServiceNative native = new WinServiceNative())
            {
                native.Lock();
                native.Open(this.ServiceName);
                native.Delete();
            }
        }

        public void Install(string arguments)
        {
            string location = Assembly.GetEntryAssembly().Location;
            if (location.Contains(" "))
                location = "\"" + location + "\"";

            string imagePath = string.IsNullOrEmpty(arguments) ? location : string.Format("{0} {1}", location, arguments);

            ApiEnums.ServiceType serviceType = ApiEnums.ServiceType.SERVICE_WIN32_OWN_PROCESS;
            using (WinServiceNative native = new WinServiceNative())
            {
                native.Lock();
                native.Create(this.ServiceName, this.DisplayName, imagePath, serviceType);
            }
        }        
    }
}
