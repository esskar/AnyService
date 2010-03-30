using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;


namespace AnyService.Win32.API
{
    public static class ApiAdvapi32
    {
        [Flags]
        public enum RegChangeNotifyFilter
        {
            /// <summary>Notify the caller if a subkey is added or deleted.</summary>
            Key = 1,
            /// <summary>Notify the caller of changes to the attributes of the key,
            /// such as the security descriptor information.</summary>
            Attribute = 2,
            /// <summary>Notify the caller of changes to a value of the key. This can
            /// include adding or deleting a value, or changing an existing value.</summary>
            Value = 4,
            /// <summary>Notify the caller of changes to the security descriptor
            /// of the key.</summary>
            Security = 8,
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegOpenKeyEx(IntPtr hKey, string subKey, uint options, int samDesired, out IntPtr phkResult);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegNotifyChangeKeyValue(IntPtr hKey, bool bWatchSubtree, RegChangeNotifyFilter dwNotifyFilter, IntPtr hEvent, bool fAsynchronous);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegCloseKey(IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int LockServiceDatabase(int hSCManager);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool UnlockServiceDatabase(int hSCManager);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern bool ChangeServiceConfigA(
            int hService, ApiEnums.ServiceType dwServiceType, int dwStartType,
            int dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup,
            int lpdwTagId, string lpDependencies, string lpServiceStartName,
            string lpPassword, string lpDisplayName);



        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern bool ChangeServiceConfig2A(
            int hService, ApiEnums.ServiceInfoLevel dwInfoLevel,
            [MarshalAs(UnmanagedType.Struct)] ref ApiStructs.ServiceDescription lpInfo);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern bool ChangeServiceConfig2A(
            int hService, ApiEnums.ServiceInfoLevel dwInfoLevel,
            [MarshalAs(UnmanagedType.Struct)] ref ApiStructs.ServiceFailureActions lpInfo);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int OpenServiceA(
            int hSCManager, string lpServiceName, ApiEnums.ServiceAccessType dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int DeleteService(int hService);


        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int CreateServiceA(
            int hSCManager,
            string lpServiceName,
            string displayName,
            ApiEnums.ServiceControlManagerType dwDesiredAccess,
            ApiEnums.ServiceType dwServiceType,
            ApiEnums.ServiceStartType dwStartType,
            ApiEnums.ServiceErrorControl dwErrorControl,
            string lpBinaryPathName, string lpLoadOrderGroup,
            string lpdwTagId,
            string lpDependencies, string lpServiceStartName, string lpPassword);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern int OpenSCManagerA(
            string lpMachineName, string lpDatabaseName, ApiEnums.ServiceControlManagerType dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool CloseServiceHandle(
            int hSCObject);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern bool QueryServiceConfigA(
            int hService, [MarshalAs(UnmanagedType.Struct)] ref ApiStructs.QueryServiceConfig lpServiceConfig,
            int cbBufSize,
            int pcbBytesNeeded);
    }
}
