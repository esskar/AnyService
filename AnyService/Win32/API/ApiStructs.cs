using System;
using System.Runtime.InteropServices;

namespace AnyService.Win32.API
{
    public static class ApiStructs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int ServiceType;
            public int CurrentState;
            public int ControlsAccepted;
            public int Win32ExitCode;
            public int ServiceSpecificExitCode;
            public int CheckPoint;
            public int WaitHint;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct QueryServiceConfig
        {
            public int ServiceType;
            public int StartType;
            public int ErrorControl;
            public string BinaryPathName;
            public string LoadOrderGroup;
            public int TagId;
            public string Dependencies;
            public string ServiceStartName;
            public string DisplayName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceAction
        {
            public ApiEnums.ServiceActionType ActionType;
            public int Delay;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceDescription
        {
            public string Description;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceFailureActions
        {
            public int ResetPeriod;
            public string RebootMsg;
            public string Command;
            public int ActionsCount;
            public int Actions;
        }
    }
}
