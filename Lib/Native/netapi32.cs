using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCmd.Lib.Native
{
    internal class netapi32
    {
        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern NET_API_STATUS NetUseEnum(
            string UncServerName,
            int Level,
            ref IntPtr Buf,
            uint PreferedMaximumSize,
            out int EntriesRead,
            out int TotalEntries,
            IntPtr resumeHandle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct USE_INFO_2
        {
            internal string ui2_local;
            internal string ui2_remote;
            internal string ui2_password;
            internal int ui2_status;
            internal int ui2_asg_type;
            internal int ui2_refcount;
            internal int ui2_usecount;
            internal string ui2_username;
            internal string ui2_domainname;
        }

        [DllImport("Netapi32.dll", SetLastError = true)]
        public static extern int NetApiBufferFree(IntPtr Buffer);

        /// <summary>
        /// Lmcons.h
        /// #define NET_API_STATUS DWORD
        /// </summary>
        public enum NET_API_STATUS : uint
        {
            NERR_Success = 0,
            /// <summary>
            /// This computer name is invalid.
            /// </summary>
            NERR_InvalidComputer = 2351,
            /// <summary>
            /// This operation is only allowed on the primary domain controller of the domain.
            /// </summary>
            NERR_NotPrimary = 2226,
            /// <summary>
            /// This operation is not allowed on this special group.
            /// </summary>
            NERR_SpeGroupOp = 2234,
            /// <summary>
            /// This operation is not allowed on the last administrative account.
            /// </summary>
            NERR_LastAdmin = 2452,
            /// <summary>
            /// The password parameter is invalid.
            /// </summary>
            NERR_BadPassword = 2203,
            /// <summary>
            /// The password does not meet the password policy requirements.
            /// Check the minimum password length, password complexity and password history requirements.
            /// </summary>
            NERR_PasswordTooShort = 2245,
            /// <summary>
            /// The user name could not be found.
            /// </summary>
            NERR_UserNotFound = 2221,
            ERROR_ACCESS_DENIED = 5,
            ERROR_NOT_ENOUGH_MEMORY = 8,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_INVALID_NAME = 123,
            ERROR_INVALID_LEVEL = 124,
            ERROR_MORE_DATA = 234,
            ERROR_SESSION_CREDENTIAL_CONFLICT = 1219,
            /// <summary>
            /// The RPC server is not available. This error is returned if a remote computer was specified in
            /// the lpServer parameter and the RPC server is not available.
            /// </summary>
            RPC_S_SERVER_UNAVAILABLE = 2147944122, // 0x800706BA
            /// <summary>
            /// Remote calls are not allowed for this process. This error is returned if a remote computer was
            /// specified in the lpServer parameter and remote calls are not allowed for this process.
            /// </summary>
            RPC_E_REMOTE_DISABLED = 2147549468 // 0x8001011C
        }


        /*
         https://stackoverflow.com/questions/926227/how-to-detect-if-machine-is-joined-to-domain
         */
        [DllImport("Netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int NetGetJoinInformation(string server, out IntPtr domain, out NetJoinStatus status);

        public enum NetJoinStatus
        {
            NetSetupUnknownStatus = 0,
            NetSetupUnjoined,
            NetSetupWorkgroupName,
            NetSetupDomainName
        }
        public static bool IsInDomain()
        {
            NetJoinStatus status = NetJoinStatus.NetSetupUnknownStatus;
            IntPtr pDomain = IntPtr.Zero;
            int result = NetGetJoinInformation(null, out pDomain, out status);
            if (pDomain != IntPtr.Zero)
            {
                NetApiBufferFree(pDomain);
            }
            if (result == 0)
            {
                return status == NetJoinStatus.NetSetupDomainName;
            }
            else
            {
                return false;
            }
        }
    }
}
