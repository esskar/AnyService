using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyService.Win32.API
{
    public static class ApiEnums
    {
        public const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        public const int GENERIC_READ = -2147483648;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int SERVICE_NO_CHANGE = -1;

        [Flags]
        public enum ServiceType
        {
            SERVICE_KERNEL_DRIVER = 0x1,
            SERVICE_FILE_SYSTEM_DRIVER = 0x2,
            SERVICE_WIN32_OWN_PROCESS = 0x10,
            SERVICE_WIN32_SHARE_PROCESS = 0x20,
            SERVICE_INTERACTIVE_PROCESS = 0x100,
            SERVICETYPE_NO_CHANGE = SERVICE_NO_CHANGE
        }

        public enum ServiceStartType : int
        {
            SERVICE_BOOT_START = 0x0,
            SERVICE_SYSTEM_START = 0x1,
            SERVICE_AUTO_START = 0x2,
            SERVICE_DEMAND_START = 0x3,
            SERVICE_DISABLED = 0x4,
            SERVICESTARTTYPE_NO_CHANGE = SERVICE_NO_CHANGE
        }

        public enum ServiceErrorControl : int
        {
            SERVICE_ERROR_IGNORE = 0x0,
            SERVICE_ERROR_NORMAL = 0x1,
            SERVICE_ERROR_SEVERE = 0x2,
            SERVICE_ERROR_CRITICAL = 0x3,
            msidbServiceInstallErrorControlVital = 0x8000,
            SERVICEERRORCONTROL_NO_CHANGE = SERVICE_NO_CHANGE
        }

        public enum ServiceStateRequest : int
        {
            SERVICE_ACTIVE = 0x1,
            SERVICE_INACTIVE = 0x2,
            SERVICE_STATE_ALL = (SERVICE_ACTIVE + SERVICE_INACTIVE)
        }

        public enum ServiceControlType : int
        {
            SERVICE_CONTROL_STOP = 0x1,
            SERVICE_CONTROL_PAUSE = 0x2,
            SERVICE_CONTROL_CONTINUE = 0x3,
            SERVICE_CONTROL_INTERROGATE = 0x4,
            SERVICE_CONTROL_SHUTDOWN = 0x5,
            SERVICE_CONTROL_PARAMCHANGE = 0x6,
            SERVICE_CONTROL_NETBINDADD = 0x7,
            SERVICE_CONTROL_NETBINDREMOVE = 0x8,
            SERVICE_CONTROL_NETBINDENABLE = 0x9,
            SERVICE_CONTROL_NETBINDDISABLE = 0xA,
            SERVICE_CONTROL_DEVICEEVENT = 0xB,
            SERVICE_CONTROL_HARDWAREPROFILECHANGE = 0xC,
            SERVICE_CONTROL_POWEREVENT = 0xD,
            SERVICE_CONTROL_SESSIONCHANGE = 0xE,
        }

        public enum ServiceState : int
        {
            SERVICE_STOPPED = 0x1,
            SERVICE_START_PENDING = 0x2,
            SERVICE_STOP_PENDING = 0x3,
            SERVICE_RUNNING = 0x4,
            SERVICE_CONTINUE_PENDING = 0x5,
            SERVICE_PAUSE_PENDING = 0x6,
            SERVICE_PAUSED = 0x7,
        }

        public enum ServiceControlAccepted : int
        {
            SERVICE_ACCEPT_STOP = 0x1,
            SERVICE_ACCEPT_PAUSE_CONTINUE = 0x2,
            SERVICE_ACCEPT_SHUTDOWN = 0x4,
            SERVICE_ACCEPT_PARAMCHANGE = 0x8,
            SERVICE_ACCEPT_NETBINDCHANGE = 0x10,
            SERVICE_ACCEPT_HARDWAREPROFILECHANGE = 0x20,
            SERVICE_ACCEPT_POWEREVENT = 0x40,
            SERVICE_ACCEPT_SESSIONCHANGE = 0x80
        }

        public enum ServiceControlManagerType : int
        {
            SC_MANAGER_CONNECT = 0x1,
            SC_MANAGER_CREATE_SERVICE = 0x2,
            SC_MANAGER_ENUMERATE_SERVICE = 0x4,
            SC_MANAGER_LOCK = 0x8,
            SC_MANAGER_QUERY_LOCK_STATUS = 0x10,
            SC_MANAGER_MODIFY_BOOT_CONFIG = 0x20,
            SC_MANAGER_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED + SC_MANAGER_CONNECT + SC_MANAGER_CREATE_SERVICE + SC_MANAGER_ENUMERATE_SERVICE + SC_MANAGER_LOCK + SC_MANAGER_QUERY_LOCK_STATUS + SC_MANAGER_MODIFY_BOOT_CONFIG
        }

        public enum ServiceAccessType : int
        {
            SERVICE_QUERY_CONFIG = 0x1,
            SERVICE_CHANGE_CONFIG = 0x2,
            SERVICE_QUERY_STATUS = 0x4,
            SERVICE_ENUMERATE_DEPENDENTS = 0x8,
            SERVICE_START = 0x10,
            SERVICE_STOP = 0x20,
            SERVICE_PAUSE_CONTINUE = 0x40,
            SERVICE_INTERROGATE = 0x80,
            SERVICE_USER_DEFINED_CONTROL = 0x100,
            SERVICE_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED + SERVICE_QUERY_CONFIG + SERVICE_CHANGE_CONFIG + SERVICE_QUERY_STATUS + SERVICE_ENUMERATE_DEPENDENTS + SERVICE_START + SERVICE_STOP + SERVICE_PAUSE_CONTINUE + SERVICE_INTERROGATE + SERVICE_USER_DEFINED_CONTROL
        }

        public enum ServiceActionType : int
        {
            SC_ACTION_NONE = 0,
            SC_ACTION_RESTART = 1,
            SC_ACTION_REBOOT = 2,
            SC_ACTION_RUN_COMMAND = 3,
        }

        public enum ServiceInfoLevel : int
        {
            SERVICE_CONFIG_DESCRIPTION = 1,
            SERVICE_CONFIG_FAILURE_ACTIONS = 2
        }
    }
}
