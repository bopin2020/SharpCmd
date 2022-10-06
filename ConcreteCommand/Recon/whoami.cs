using SharpCmd.Contract;
using SharpCmd.Lib.Help;
using SharpCmd.Lib.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

using static SharpCmd.Lib.Native.advapi32;
using static SharpCmd.Lib.Native.kernel32;

namespace SharpCmd.ConcreteCommand.Recon
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/windows/win32/api/securitybaseapi/nf-securitybaseapi-privilegecheck
    /// sddl.h
    /// securitybase.h
    /// </summary>
    internal partial class whoami : IContract
    {
        public string CommandName => "whoami";

        public void Execute(Dictionary<string, string> arguments)
        {
            if (arguments.ContainsKey("/?"))
            {
                string help = @"
whoami
        /user /logonid /priv /groups /all
";
                Console.WriteLine(help);
                return;
            }

            bool domainMachine;
            bool upn;
            bool fqdn;
            bool logonid;
            bool priv;
            bool user;
            bool all;
            if(arguments.ContainsKey("whoami") && arguments.Count == 1)
            {
                Console.WriteLine(GetUserName());
                return;
            }

            if(arguments.ContainsKey("/user"))
            {
                InitUser();
                return;
            }

            if (arguments.ContainsKey("/logonid"))
            {
                InitLogonId();
                return;
            }

            if (arguments.ContainsKey("/priv"))
            {
                InitPriv();
                return;
            }

            if (arguments.ContainsKey("/groups"))
            {
                InitGroups();
                return;
            }

            if(arguments.ContainsKey("/all"))
            {
                InitUser();
                InitGroups();
                InitPriv();
            }
        }
    }
    internal partial class whoami
    {
        private void InitUser()
        {
            Console.WriteLine("UserName:\t" + GetUserName());
            Console.WriteLine("SID:\t" + currentIdentity.User.Value);
            Console.WriteLine();
        }

        private void InitLogonId()
        {
            uint TokenInfLength = 0;
            bool Result;

            // first call gets lenght of TokenInformation
            Result = GetTokenInformation(WindowsIdentity.GetCurrent().Token, TOKEN_INFORMATION_CLASS.TokenLogonSid, IntPtr.Zero, TokenInfLength, out TokenInfLength);
            IntPtr TokenInformation = Marshal.AllocHGlobal(new IntPtr(TokenInfLength));

            Result = GetTokenInformation(WindowsIdentity.GetCurrent().Token, TOKEN_INFORMATION_CLASS.TokenLogonSid, TokenInformation, TokenInfLength, out TokenInfLength);
            if (Result)
            {
                TOKEN_GROUPS tokenGroups = (TOKEN_GROUPS)Marshal.PtrToStructure(TokenInformation, typeof(TOKEN_GROUPS));
                string sid;
                if (ConvertSidToStringSid(tokenGroups.Groups[0].Sid, out sid))
                {
                    Console.WriteLine(sid);
                }
            }
            Console.WriteLine();

        }

        private void InitPriv()
        {
            /*
                 To get a list of all the enabled and disabled privileges held by an access token, call the GetTokenInformation function.
                 */
            uint TokenInfLength = 0;
            bool Result;

            // first call gets lenght of TokenInformation
            Result = GetTokenInformation(WindowsIdentity.GetCurrent().Token, TOKEN_INFORMATION_CLASS.TokenPrivileges, IntPtr.Zero, TokenInfLength, out TokenInfLength);

            IntPtr TokenInformation = Marshal.AllocHGlobal(new IntPtr(TokenInfLength));

            Result = GetTokenInformation(WindowsIdentity.GetCurrent().Token, TOKEN_INFORMATION_CLASS.TokenPrivileges, TokenInformation, TokenInfLength, out TokenInfLength);
            if (Result)
            {
                TOKEN_PRIVILEGES tokenPrives = (TOKEN_PRIVILEGES)Marshal.PtrToStructure(TokenInformation, typeof(TOKEN_PRIVILEGES));
                for (int i = 0; i < tokenPrives.Privileges.Length; i++)
                {
                    int luidNameLen = 0;
                    IntPtr ptrLuid = Marshal.AllocHGlobal(Marshal.SizeOf(tokenPrives.Privileges[i].Luid));
                    Marshal.StructureToPtr(tokenPrives.Privileges[i].Luid, ptrLuid, true);
                    LookupPrivilegeName(null, ptrLuid, null, ref luidNameLen);
                    StringBuilder sb = new StringBuilder(luidNameLen);
                    if (LookupPrivilegeName(null, ptrLuid, sb, ref luidNameLen))
                    {
                        /*
                         0 - privilege not enabled
                        2 - priviledge is enabled
                        3 - priviledge is enabled by default (1+2)
                         */
                        Console.WriteLine(sb.ToString() + Constant.T + PrivilegeStatus(tokenPrives.Privileges[i].Attributes));
                    }
                }
            }

            Marshal.FreeHGlobal(TokenInformation);
            Console.WriteLine();

        }

        private void InitGroups()
        {
            foreach (IdentityReference identityReference in currentIdentity.Groups)
            {
                SecurityIdentifier secId = identityReference as SecurityIdentifier;
                Console.WriteLine(GetSidType(secId) + Constant.T + secId.Value);
            }
            Console.WriteLine("IntegrityLevel:\t" + GetIntegrityLevel());
            Console.WriteLine();

        }
    }

    internal partial class whoami
    {

        WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
        SecurityIdentifier securityIdentifier = WindowsIdentity.GetCurrent().Owner;

        private string PrivilegeStatus(uint value)
        {
            string status = null;
            switch (value)
            {
                case 0:
                    status = "Disable";
                    break;
                case 2:
                    status = "Enable";
                    break;
                default:
                    status = "Enable by Default";
                    break;
            }
            return status;
        }

        private WellKnownSidType GetSidType(SecurityIdentifier securityIdentifier)
        {
            for (int i = 0; i < 60; i++)
            {
                if(securityIdentifier.IsWellKnown((WellKnownSidType)(i)))
                {
                    return (WellKnownSidType)i;
                }
            }
            return WellKnownSidType.NullSid;
        }

        private string GetUserName()
        {
#if pinvoke
            // GetUserNameExW
            StringBuilder sb = new StringBuilder(256);
            int usernameSize = 0;
            // workspace
            //if the user account is not in a domain,only NameSamCompatible is supported
            secur32.GetUserNameEx(ExtendedNameFormat.NameSamCompatible, sb, ref usernameSize);
            secur32.GetUserNameEx(ExtendedNameFormat.NameSamCompatible, sb, ref usernameSize);

            // domain
            return sb.ToString();
#elif Environment
            return Environment.MachineName + Constant.SpecialSlash + Environment.UserName;

#elif !WindowsIdentity
            return currentIdentity.Name;
#else

#endif
        }

        private IntegrityLevel GetIntegrityLevel()
        {
            IntPtr pId = (Process.GetCurrentProcess().Handle);

            IntPtr hToken = IntPtr.Zero;
            if (OpenProcessToken(pId, TOKEN_QUERY, out hToken))
            {
                try
                {
                    IntPtr pb = Marshal.AllocCoTaskMem(1000);
                    try
                    {
                        uint cb = 1000;
                        if (GetTokenInformation(hToken, TOKEN_INFORMATION_CLASS.TokenIntegrityLevel, pb, cb, out cb))
                        {
                            IntPtr pSid = Marshal.ReadIntPtr(pb);

                            int dwIntegrityLevel = Marshal.ReadInt32(GetSidSubAuthority(pSid, (Marshal.ReadByte(GetSidSubAuthorityCount(pSid)) - 1U)));

                            if (dwIntegrityLevel == SECURITY_MANDATORY_LOW_RID)
                            {
                                return IntegrityLevel.Low;
                            }
                            else if (dwIntegrityLevel >= SECURITY_MANDATORY_MEDIUM_RID && dwIntegrityLevel < SECURITY_MANDATORY_HIGH_RID)
                            {
                                // Medium Integrity
                                return IntegrityLevel.Medium;
                            }
                            else if (dwIntegrityLevel >= SECURITY_MANDATORY_SYSTEM_RID)
                            {
                                // System Integrity
                                return IntegrityLevel.System;
                            }
                            else if (dwIntegrityLevel >= SECURITY_MANDATORY_HIGH_RID)
                            {
                                // High Integrity
                                return IntegrityLevel.High;
                            }

                            return IntegrityLevel.None;
                        }
                        else
                        {
                            int errno = Marshal.GetLastWin32Error();
                            if (errno == ERROR_INVALID_PARAMETER)
                            {
                                throw new NotSupportedException();
                            }
                            throw new Win32Exception(errno);
                        }
                    }
                    finally
                    {
                        Marshal.FreeCoTaskMem(pb);
                    }
                }
                finally
                {
                    CloseHandle(hToken);
                }
            }
            {
                int errno = Marshal.GetLastWin32Error();
                throw new Win32Exception(errno);
            }
        }

        private bool IsGuest()
        {
            return currentIdentity.IsGuest;
        }

        private bool IsSystem()
        {
            return currentIdentity.IsSystem;
        }

        /// <summary>
        /// NTLM
        /// Kerberos
        /// </summary>
        /// <returns></returns>
        private string GetAuthType()
        {
            return currentIdentity.AuthenticationType;
        }

        private IntPtr GetCurrentUserToken()
        {
            return currentIdentity.Token;
        }

        private bool IsAuthed()
        {
            return currentIdentity.IsAuthenticated;
        }
    }
}
