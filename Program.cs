using System;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NDesk.Options;

namespace desktop_shiv
{
    class Program
    {
        //
        // Imported winAPI functions.
        //
        [DllImport("user32.dll", EntryPoint = "CreateDesktop", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateDesktop(
                        [MarshalAs(UnmanagedType.LPWStr)] string desktopName,
                        [MarshalAs(UnmanagedType.LPWStr)] string device, // must be null.
                        [MarshalAs(UnmanagedType.LPWStr)] string deviceMode, // must be null,
                        [MarshalAs(UnmanagedType.U4)] int flags,  // use 0
                        [MarshalAs(UnmanagedType.U4)] ACCESS_MASK accessMask,
                        IntPtr attributes);


        // ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.WIN32COM.v10.en/dllproc/base/closedesktop.htm
        [DllImport("user32.dll", EntryPoint = "CloseDesktop", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseDesktop(IntPtr handle);

        [DllImport("user32.dll", EntryPoint = "SwitchDesktop", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SwitchDesktop(IntPtr hDesktop);

        [DllImport("user32.dll", EntryPoint = "SetThreadDesktop", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport("user32.dll", EntryPoint = "GetThreadDesktop", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetThreadDesktop(int dwThreadId);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WaitForSingleObject(IntPtr handle, UInt32 dwMillis);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", EntryPoint = "GetExitCodeProcess", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetExitCodeProcess(IntPtr hProcess, out int lpExitCode);

        [DllImport("kernel32.dll")]
        static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            [MarshalAs(UnmanagedType.U4)] PROCESS_CREATION_FLAGS dwCreationFlags, //uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [Flags]
        public enum ACCESS_MASK : uint
        {
            DELETE = 0x00010000,
            READ_CONTROL = 0x00020000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            SYNCHRONIZE = 0x00100000,

            STANDARD_RIGHTS_REQUIRED = 0x000F0000,

            STANDARD_RIGHTS_READ = 0x00020000,
            STANDARD_RIGHTS_WRITE = 0x00020000,
            STANDARD_RIGHTS_EXECUTE = 0x00020000,

            STANDARD_RIGHTS_ALL = 0x001F0000,

            SPECIFIC_RIGHTS_ALL = 0x0000FFFF,

            ACCESS_SYSTEM_SECURITY = 0x01000000,

            MAXIMUM_ALLOWED = 0x02000000,

            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000,

            DESKTOP_READOBJECTS = 0x00000001,
            DESKTOP_CREATEWINDOW = 0x00000002,
            DESKTOP_CREATEMENU = 0x00000004,
            DESKTOP_HOOKCONTROL = 0x00000008,
            DESKTOP_JOURNALRECORD = 0x00000010,
            DESKTOP_JOURNALPLAYBACK = 0x00000020,
            DESKTOP_ENUMERATE = 0x00000040,
            DESKTOP_WRITEOBJECTS = 0x00000080,
            DESKTOP_SWITCHDESKTOP = 0x00000100,

            WINSTA_ENUMDESKTOPS = 0x00000001,
            WINSTA_READATTRIBUTES = 0x00000002,
            WINSTA_ACCESSCLIPBOARD = 0x00000004,
            WINSTA_CREATEDESKTOP = 0x00000008,
            WINSTA_WRITEATTRIBUTES = 0x00000010,
            WINSTA_ACCESSGLOBALATOMS = 0x00000020,
            WINSTA_EXITWINDOWS = 0x00000040,
            WINSTA_ENUMERATE = 0x00000100,
            WINSTA_READSCREEN = 0x00000200,

            WINSTA_ALL_ACCESS = 0x0000037F
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public unsafe byte* lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [Flags]
        internal enum PROCESS_CREATION_FLAGS : uint
        {
            ZERO_FLAG = 0x00000000,
            CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
            CREATE_DEFAULT_ERROR_MODE = 0x04000000,
            CREATE_NEW_CONSOLE = 0x00000010,
            CREATE_NEW_PROCESS_GROUP = 0x00000200,
            CREATE_NO_WINDOW = 0x08000000,
            CREATE_PROTECTED_PROCESS = 0x00040000,
            CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
            CREATE_SEPARATE_WOW_VDM = 0x00001000,
            CREATE_SHARED_WOW_VDM = 0x00001000,
            CREATE_SUSPENDED = 0x00000004,
            CREATE_UNICODE_ENVIRONMENT = 0x00000400,
            DEBUG_ONLY_THIS_PROCESS = 0x00000002,
            DEBUG_PROCESS = 0x00000001,
            DETACHED_PROCESS = 0x00000008,
            EXTENDED_STARTUPINFO_PRESENT = 0x00080000,
            INHERIT_PARENT_AFFINITY = 0x00010000
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STARTUPINFO
        {
            public int cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        private const UInt32 WAIT_INFINITE = 0xFFFFFFFF;
        private const int NORMAL_PRIORITY_CLASS = 0x00000020;
        private const int OPTS_ERROR = 255;

        static int Main(string[] args)
        {
            // Main return code. The returncode of the application will be stored here and returned from main at the end.
            int main_retc=0;

            // Options that will be set
            bool verbose_flag = false;
            bool show_help = false;
            bool dont_create_desktop = false;
            string desktop_name = "DesktopShiv";
            string command_line = "";
            UInt32 timeout=0;
            UInt32 processWaitTime = WAIT_INFINITE;

            // Option parser using NDesk.Options (source in Options.cs)
            var p = new OptionSet() {
                { "n|name=", "The name of the desktop you will run your command in. [Defaults to DesktopShiv]", v => { if (v != null) desktop_name=v; } },
                { "t|timeout=", "Timeout in seconds waiting for the user to respond on the secondary desktop. [Defaults to INFINITY]", (UInt32 v) => { timeout = v; } },
                { "h|help", "Show this message and exit", v => show_help = v != null },
                { "s|suppress", "Dont actually create the alternate desktop. Useful for testing your command_line if necessary.", v => dont_create_desktop = v != null },
                { "d|debug", "Print debug logging stuff.", v => verbose_flag = v != null },
            };

            // Extra options are pushed into this list collection
            List<string> extra;
            try
            {
                extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("Desktop-Shiv:");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `Desktop-Shiv --help' for more information");
                return OPTS_ERROR;
            }
            
            
            // Options are parsed now, now we take the action that we need
            if (show_help)
            {
                ShowHelp(p);
                return 0;
            }

            // Ensure timeouts stay within bounds
            if (timeout > 0 && timeout < 4294967) {
                processWaitTime = timeout * 1000;

            } else if (timeout > 0) {
                Console.WriteLine("Desktop-Shiv:");
                Console.WriteLine("Timeout is too high! You can wait up to 4294966 seconds.");
                return OPTS_ERROR;
            }

            // Ensure we actually have a command to execute!
            if (extra.Count < 1)
            {
                Console.WriteLine("Desktop-Shiv:");
                Console.WriteLine("No command line specified");
                Console.WriteLine("Try `Desktop-Shiv --help` for more information");
                return OPTS_ERROR;
            }
            command_line = string.Join(" ", extra.ToArray() );

            // Print out the actions of the application, if requested (verbose flag)
            if (verbose_flag)
            {
                if (!dont_create_desktop)
                {
                    Console.WriteLine(String.Format("Creating desktop: \"{0}\"", desktop_name));
                }
                else
                {
                    Console.WriteLine("Spawning process in current desktop.");
                }

                Console.WriteLine(String.Format("Executing: {0}", command_line));
                if (processWaitTime == WAIT_INFINITE)
                {
                    Console.WriteLine("Timout: INFINITY");
                }
                else
                {
                    Console.WriteLine(String.Format("Timeout: {0} seconds before returning to the main desktop.", timeout));
                    Console.WriteLine("  -- You will need to make sure that the program you start is killed.");
                }

                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            main_retc = CreateProcessInNewDesktop(command_line, desktop_name, processWaitTime, dont_create_desktop);

            if (verbose_flag)
            {
                Console.Write("Press any key to continue");
                Console.ReadKey();
            }

            return main_retc;
        }

        static int CreateProcessInNewDesktop(string command_line, string desktop_name, UInt32 timeout, bool dont_create_desktop)
        {
            int iProcessReturnCode = 0;

            // Get our default (main) desktop
            IntPtr hDefaultDesktop = GetThreadDesktop(GetCurrentThreadId());

            // Specify a desktop handle
            IntPtr hShivDesktop = IntPtr.Zero;
            if (!dont_create_desktop)
            {
                // Create the desktop
                hShivDesktop = CreateDesktop(desktop_name, null, null, 0, ACCESS_MASK.GENERIC_ALL, IntPtr.Zero);
                
                // Probably need more error checking just here..

                // Switch to the Shiv Desktop
                SwitchDesktop(hShivDesktop);
            }

            // Create our process inside the shiv-desktop
            try
            {
                // We have to use the Win32 api to create the process so that we can pin it to a desktop...
                STARTUPINFO si = new STARTUPINFO();
                PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
                
                if (!dont_create_desktop)
                {
                    si.lpDesktop = desktop_name;
                }

                // Create the process
                CreateProcess(null, command_line, IntPtr.Zero, IntPtr.Zero, false, PROCESS_CREATION_FLAGS.CREATE_NEW_CONSOLE, IntPtr.Zero, null, ref si, out pi);
                
                // Wait for the process to exit up to timeout milliseconds
                WaitForSingleObject(pi.hProcess, timeout);

                // Get the exit code to return
                GetExitCodeProcess(pi.hProcess, out iProcessReturnCode);

                Console.WriteLine(String.Format("Command exited with returncode {0:D}", iProcessReturnCode));

                // Close our handles to the process
                CloseHandle(pi.hProcess);
                CloseHandle(pi.hThread);

            }
            catch
            {
                Console.WriteLine("An error occurred creating the process.");
            }

            // Switch back to the default desktop
            SwitchDesktop(hDefaultDesktop);

            if (!dont_create_desktop)
            {
                // Clean up the shiv desktop
                CloseDesktop(hShivDesktop);
            }

            return iProcessReturnCode;
        }


        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: Desktop-Shiv [OPTIONS]+ Command Line");
            Console.WriteLine("Open an application on a new Win32 desktop.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
