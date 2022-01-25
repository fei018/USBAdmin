using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

namespace USBNotifyLib.Win32API
{
    //// Ref: https://stackoverflow.com/questions/19776716/c-sharp-windows-service-creates-process-but-doesnt-executes-it
    ///  https://www.cnblogs.com/gnielee/archive/2010/04/08/session0-isolation-part2.html
    
    [SuppressUnmanagedCodeSecurity]
    public class ProcessApiHelp
    {
        #region Params
        private const int GENERIC_ALL_ACCESS = 0x10000000;
        //public const int CREATE_NO_WINDOW = 0x08000000;
        private const int STARTF_USESHOWWINDOW = 0x00000001;

        private const int SE_PRIVILEGE_ENABLED = 0x00000002;
        private const string SE_INCREASE_QUOTA_NAME = "SeIncreaseQuotaPrivilege";
        internal const string SE_TCB_NAME = "SeTcbPrivilege";


        enum CreateProcessFlags
        {
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_NO_WINDOW = 0x08000000,
            CREATE_PROTECTED_PROCESS = 0x00040000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_SEPARATE_WOW_VDM = 0x00000800,
            CREATE_SHARED_WOW_VDM = 0x00001000,
            CREATE_SUSPENDED = 0x00000004,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,
            DEBUG_PROCESS = 0x00000001,
            DETACHED_PROCESS = 0x00000008,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
            INHERIT_PARENT_AFFINITY = 0x00010000
        }


        enum TOKEN_INFORMATION_CLASS
        {

            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        private enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        private enum TOKEN_TYPE
        {
            TokenPrimary = 1,
            TokenImpersonation
        }
        #endregion

        #region + public static int GetCurrentUserSessionID()
        /// <summary>
        /// system = 0, error = -1
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentUserSessionID()
        {
            uint dwSessionId = (uint)ProcessApi.WTSGetActiveConsoleSessionId();

            // gets the Id of the User logged in with WinLogOn
            Process[] processes = Process.GetProcessesByName("winlogon");
            foreach (Process p in processes)
            {
                if ((uint)p.SessionId == dwSessionId)
                {

                    //　this is the process controlled by the same sessionID
                    return p.SessionId;
                }
            }

            return -1;
        }
        #endregion

        #region + public static Process CreateProcessAsUser(string filePath, string args)
        /// <summary>
        /// Main method for Create process used advapi32: CreateProcessAsUser
        /// </summary>
        /// <param name="filePath">Execute path, for example: c:\app\myapp.exe</param>
        /// <param name="args">Arugments passing to execute application</param>
        /// <returns>Process just been created</returns>
        public static Process CreateProcessAsUser(string filePath, string args)
        {

            var dupedToken = IntPtr.Zero;

            var pi = new ProcessApi.PROCESS_INFORMATION();
            var sa = new ProcessApi.SECURITY_ATTRIBUTES();
            sa.Length = Marshal.SizeOf(sa);

            try
            {
                // get current token
                var token = WindowsIdentity.GetCurrent().Token;

                var si = new ProcessApi.STARTUPINFO();
                si.cb = Marshal.SizeOf(si);
                si.lpDesktop = "";
                si.dwFlags = STARTF_USESHOWWINDOW;

                var dir = Path.GetDirectoryName(filePath);
                var fileName = Path.GetFileName(filePath);

                // Create new access token for current token
                if (!ProcessApi.DuplicateTokenEx(
                    token,
                    GENERIC_ALL_ACCESS,
                    ref sa,
                    (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                    (int)TOKEN_TYPE.TokenPrimary,
                    ref dupedToken
                ))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                // got the session Id from user level
                uint curSessionid = (uint)GetCurrentUserSessionID();

                // retrieve the primary access token for the user associated with the specified session Id.
                if (!ProcessApi.WTSQueryUserToken(curSessionid, out dupedToken))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                WindowsIdentity.RunImpersonated(WindowsIdentity.GetCurrent().AccessToken, () =>
                {
                    if (!ProcessApi.CreateProcessAsUser(
                                          dupedToken, // user token
                                          filePath, // app name or path
                                          string.Format("\"{0}\" {1}", fileName.Replace("\"", "\"\""), args), // command line
                                          ref sa, // process attributes
                                          ref sa, // thread attributes
                                          false, // do not inherit handles
                                          (int)CreateProcessFlags.CREATE_NEW_CONSOLE, //flags
                                          IntPtr.Zero, // environment block
                                          dir, // current dir
                                          ref si, // startup info
                                          ref pi // process info
                                  ))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                });

                return Process.GetProcessById(pi.dwProcessID);
            }
            finally
            {
                // close all open resource
                if (pi.hProcess != IntPtr.Zero)
                    ProcessApi.CloseHandle(pi.hProcess);
                if (pi.hThread != IntPtr.Zero)
                    ProcessApi.CloseHandle(pi.hThread);
                if (dupedToken != IntPtr.Zero)
                    ProcessApi.CloseHandle(dupedToken);
            }
        }
        #endregion
    }
}
