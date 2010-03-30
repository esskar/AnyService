using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using AnyService.Win32.API;

namespace AnyService.Win32
{
    public class WinServiceNative : Disposable
    {
        private int m_manager = 0, m_lock = 0, m_service = 0;

        public WinServiceNative()
        {
            m_manager = ApiAdvapi32.OpenSCManagerA(null, null, ApiEnums.ServiceControlManagerType.SC_MANAGER_ALL_ACCESS);
            if (m_manager <= 0)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to open the Services Manager.");            
        }

        public ApiStructs.ServiceStatus QueryStatus()
        {            
            if (!this.IsOpen)
                throw new Win32Exception("Service not yet open.");
                            
            ApiStructs.ServiceStatus status = new ApiStructs.ServiceStatus();

            int bytesNeeded = 0;
            if(!ApiAdvapi32.QueryServiceStatusEx(m_service, 0, ref status, Marshal.SizeOf(status), ref bytesNeeded))
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to query service status.");                        
            return status;
        }

        public void Lock()
        {
            if (m_lock == 0)
            {
                m_lock = ApiAdvapi32.LockServiceDatabase(m_manager);
                if (m_lock <= 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to lock the Services Manager.");
            }
        }

        public void Unlock()
        {
            if (m_lock != 0)
            {
                ApiAdvapi32.UnlockServiceDatabase(m_lock);
                m_lock = 0;
            }
        }

        public void Open(string name)
        {
            this.Close();
            m_service = ApiAdvapi32.OpenServiceA(m_manager, name, ApiEnums.ServiceAccessType.SERVICE_ALL_ACCESS);
            if (m_service <= 0)
                throw new Win32Exception("Service does not exist.");
        }

        public bool IsOpen { get { return m_service != 0; } }

        public void Create(string name, string displayName, string imagePath, ApiEnums.ServiceType serviceType)
        {
            this.Close();

            m_service = ApiAdvapi32.CreateServiceA(
                    m_manager,
                    name,
                    displayName,
                    ApiEnums.ServiceControlManagerType.SC_MANAGER_ALL_ACCESS,
                    serviceType,
                    ApiEnums.ServiceStartType.SERVICE_AUTO_START,
                    ApiEnums.ServiceErrorControl.SERVICE_ERROR_NORMAL,
                    imagePath,
                    null,
                    null,
                    null, null, null);

            if (m_service == 0)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to install the service.");    
        }

        public void Start()
        {
            if(this.IsOpen)
                if(!ApiAdvapi32.StartService(m_service, 0, IntPtr.Zero))
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to start the service.");    

        }

        public void Stop()
        {
        }

        public void Delete()
        {
            if(this.IsOpen)
                ApiAdvapi32.DeleteService(m_service);
        }

        public void Close()
        {
            if (m_service != 0)
            {
                ApiAdvapi32.CloseServiceHandle(m_service);
                m_service = 0;
            }
        }

        protected override void Release()
        {
            this.Close();
            this.Unlock();

            if (m_manager != 0)
            {
                ApiAdvapi32.CloseServiceHandle(m_manager);
                m_manager = 0;
            }
            base.Release();
        }
    }
}
