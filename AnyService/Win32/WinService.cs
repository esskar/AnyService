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
        

        public void Uninstall()
        {

            int handle = 0, lockHandle = 0, serviceHandle = 0;

            try
            {
                handle = ApiAdvapi32.OpenSCManagerA(null, null, ApiEnums.ServiceControlManagerType.SC_MANAGER_ALL_ACCESS);
                if (handle <= 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to open the Services Manager.");

                lockHandle = ApiAdvapi32.LockServiceDatabase(handle);
                if (lockHandle <= 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to lock the Services Manager.");

                serviceHandle = ApiAdvapi32.OpenServiceA(handle, this.ServiceName, ApiEnums.ServiceAccessType.SERVICE_ALL_ACCESS);
                if (serviceHandle <= 0)
                    throw new Win32Exception("Service does not exist.");

                ApiAdvapi32.DeleteService(serviceHandle);
            }
            finally
            {
                if (serviceHandle > 0)
                    ApiAdvapi32.CloseServiceHandle(serviceHandle);

                if (lockHandle > 0)
                    ApiAdvapi32.UnlockServiceDatabase(lockHandle);

                if (handle > 0)
                    ApiAdvapi32.CloseServiceHandle(handle);
            }
        }

        public void Install(bool interactive)
        {
            int handle = 0, lockHandle = 0, serviceHandle = 0;

            try
            {
                handle = ApiAdvapi32.OpenSCManagerA(null, null, ApiEnums.ServiceControlManagerType.SC_MANAGER_ALL_ACCESS);
                if (handle <= 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to open the Services Manager.");

                lockHandle = ApiAdvapi32.LockServiceDatabase(handle);
                if (lockHandle <= 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to lock the Services Manager.");

                serviceHandle = ApiAdvapi32.OpenServiceA(handle, this.ServiceName, ApiEnums.ServiceAccessType.SERVICE_ALL_ACCESS);
                if (serviceHandle > 0) // service does exist
                    throw new Win32Exception("Service already exists.");

                string location = Assembly.GetEntryAssembly().Location;
                if (location.Contains(" "))
                    location = "\"" + location + "\"";

                string serviceNameArgument = this.ServiceName;
                if(serviceNameArgument.Contains(" "))
                    serviceNameArgument = "\"" + serviceNameArgument + "\"";

                ApiEnums.ServiceType serviceType = ApiEnums.ServiceType.SERVICE_WIN32_OWN_PROCESS;
                if (interactive)
                    serviceType |= ApiEnums.ServiceType.SERVICE_INTERACTIVE_PROCESS;


                serviceHandle = ApiAdvapi32.CreateServiceA(
                    handle,
                    this.ServiceName,
                    this.DisplayName,
                    ApiEnums.ServiceControlManagerType.SC_MANAGER_ALL_ACCESS,
                    serviceType,
                    ApiEnums.ServiceStartType.SERVICE_AUTO_START,
                    ApiEnums.ServiceErrorControl.SERVICE_ERROR_NORMAL,
                    string.Format("{0} run {1}", location, serviceNameArgument),
                    null,
                    null,
                    null, null, null);

                if (serviceHandle == 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to install the Service.");                
            }
            finally
            {
                if (serviceHandle > 0)
                    ApiAdvapi32.CloseServiceHandle(serviceHandle);

                if (lockHandle > 0)
                    ApiAdvapi32.UnlockServiceDatabase(lockHandle);

                if (handle > 0)
                    ApiAdvapi32.CloseServiceHandle(handle);
            }

        }        
    }
}
