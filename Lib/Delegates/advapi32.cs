using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using static SharpCmd.Lib.Native.advapi32;

namespace SharpCmd.Lib.Delegates
{
    public delegate bool PrivilegeCheck(IntPtr ClientToken,ref PRIVILEGE_SET RequiredPrivileges,out bool pfResult);

    public delegate bool LookupPrivilegeName(string lpSystemName,IntPtr lpLuid,StringBuilder lpName,ref int cchName);

    public delegate bool LogonUser(string lpszUsername,string lpszDomain,string lpszPassword,int dwLogonType,int dwLogonProvider,out IntPtr phToken);

    public delegate bool CreateProcessWithTokenW(IntPtr hToken, LogonFlags dwLogonFlags,string lpApplicationName, string lpCommandLine,int dwCreationFlags, IntPtr lpEnvironment,IntPtr lpCurrentDirectory, [In] ref STARTUPINFO lpStartupInfo,out PROCESS_INFORMATION lpProcessInformation);

    public delegate bool OpenProcessToken(IntPtr ProcessHandle,UInt32 DesiredAccess, out IntPtr TokenHandle);

    public delegate bool OpenThreadToken(IntPtr ThreadHandle,uint DesiredAccess,bool OpenAsSelf,out IntPtr TokenHandle);

    public delegate bool DuplicateTokenEx(IntPtr hExistingToken,uint dwDesiredAccess,ref SECURITY_ATTRIBUTES lpTokenAttributes,SECURITY_IMPERSONATION_LEVEL ImpersonationLevel,TOKEN_TYPE TokenType,out IntPtr phNewToken);

    public delegate bool GetTokenInformation(IntPtr TokenHandle,TOKEN_INFORMATION_CLASS TokenInformationClass,IntPtr TokenInformation,uint TokenInformationLength,out uint ReturnLength);

    public delegate bool ConvertSidToStringSid(IntPtr pSid, out string strSid);

    public delegate bool LookupPrivilegeValue(string systemName, string privilegeName, ref LUID luid);

    public delegate IntPtr GetSidSubAuthority(IntPtr sid, UInt32 subAuthorityIndex);

    public delegate IntPtr GetSidSubAuthorityCount(IntPtr sid);

    public class advapi32
    {
        public static bool PrivilegeCheck(IntPtr ClientToken,ref PRIVILEGE_SET RequiredPrivileges,out bool pfResult)
        {
            PrivilegeCheck anonymous = MiniDInvoke.GetFunctionPointer<PrivilegeCheck>("advapi32", "PrivilegeCheck");
            return anonymous(ClientToken, ref RequiredPrivileges, out pfResult);
        }

        public static bool LookupPrivilegeName(string lpSystemName, IntPtr lpLuid, StringBuilder lpName, ref int cchName)
        {
            LookupPrivilegeName anonymous = MiniDInvoke.GetFunctionPointer<LookupPrivilegeName>("advapi32", "LookupPrivilegeName");
            return anonymous(lpSystemName, lpLuid, lpName,ref cchName);
        }

        public static bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken)
        {
            LogonUser anonymous = MiniDInvoke.GetFunctionPointer<LogonUser>("advapi32", "LogonUser");
            return anonymous(lpszUsername, lpszDomain, lpszPassword, dwLogonType, dwLogonProvider,out phToken);
        }

        public static bool CreateProcessWithTokenW(IntPtr hToken, LogonFlags dwLogonFlags, string lpApplicationName, string lpCommandLine, int dwCreationFlags, IntPtr lpEnvironment, IntPtr lpCurrentDirectory, [In] ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation)
        {
            CreateProcessWithTokenW anonymous = MiniDInvoke.GetFunctionPointer<CreateProcessWithTokenW>("advapi32", "CreateProcessWithTokenW");
            return anonymous(hToken, dwLogonFlags, lpApplicationName, lpCommandLine, dwCreationFlags, lpEnvironment, lpCurrentDirectory,ref lpStartupInfo,out lpProcessInformation);
        }

        public static bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle)
        {
            OpenProcessToken anonymous = MiniDInvoke.GetFunctionPointer<OpenProcessToken>("advapi32", "OpenProcessToken");
            return anonymous(ProcessHandle, DesiredAccess, out TokenHandle);
        }

        public static bool OpenThreadToken(IntPtr ThreadHandle, uint DesiredAccess, bool OpenAsSelf, out IntPtr TokenHandle)
        {
            OpenThreadToken anonymous = MiniDInvoke.GetFunctionPointer<OpenThreadToken>("advapi32", "OpenThreadToken");
            return anonymous(ThreadHandle, DesiredAccess, OpenAsSelf, out TokenHandle);
        }

        public static bool DuplicateTokenEx(IntPtr hExistingToken, uint dwDesiredAccess, ref SECURITY_ATTRIBUTES lpTokenAttributes, SECURITY_IMPERSONATION_LEVEL ImpersonationLevel, TOKEN_TYPE TokenType, out IntPtr phNewToken)
        {
            DuplicateTokenEx anonymous = MiniDInvoke.GetFunctionPointer<DuplicateTokenEx>("advapi32", "DuplicateTokenEx");
            return anonymous(hExistingToken, dwDesiredAccess, ref lpTokenAttributes, ImpersonationLevel, TokenType,out phNewToken);
        }

        public static bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS TokenInformationClass, IntPtr TokenInformation, uint TokenInformationLength, out uint ReturnLength)
        {
            GetTokenInformation anonymous = MiniDInvoke.GetFunctionPointer<GetTokenInformation>("advapi32", "GetTokenInformation");
            return anonymous(TokenHandle, TokenInformationClass, TokenInformation, TokenInformationLength,out ReturnLength);
        }

        public static bool ConvertSidToStringSid(IntPtr pSid, out string strSid)
        {
            ConvertSidToStringSid anonymous = MiniDInvoke.GetFunctionPointer<ConvertSidToStringSid>("advapi32", "ConvertSidToStringSid");
            return anonymous(pSid, out strSid);
        }

        public static bool LookupPrivilegeValue(string systemName, string privilegeName, ref LUID luid)
        {
            LookupPrivilegeValue anonymous = MiniDInvoke.GetFunctionPointer<LookupPrivilegeValue>("advapi32", "LookupPrivilegeValue");
            return anonymous(systemName, privilegeName, ref luid);
        }

        public static IntPtr GetSidSubAuthority(IntPtr sid, UInt32 subAuthorityIndex)
        {
            GetSidSubAuthority anonymous = MiniDInvoke.GetFunctionPointer<GetSidSubAuthority>("advapi32", "GetSidSubAuthority");
            return anonymous(sid, subAuthorityIndex);
        }

        public static IntPtr GetSidSubAuthorityCount(IntPtr sid)
        {
            GetSidSubAuthorityCount anonymous = MiniDInvoke.GetFunctionPointer<GetSidSubAuthorityCount>("advapi32", "GetSidSubAuthorityCount");
            return anonymous(sid);
        }

        public static bool LookupPrivilegeValueEx(string systemName, PrivilegeNames privilegeName, ref LUID luid)
        {
            return LookupPrivilegeValue(systemName, privilegeName.ToString(), ref luid);
        }
    }
}
