using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TopHack.Other
{
    internal static class Globals
    {
        internal class Imports
        {
            public static bool IsWindowFocues(Process procName)
            {
                IntPtr activatedHandle = Imports.GetForegroundWindow();
                if (activatedHandle == IntPtr.Zero) return false;
                Imports.GetWindowThreadProcessId(activatedHandle, out int activeProcId);
                return activeProcId == procName.Id;
            }

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

            [DllImport("kernel32.dll")]
            public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

            [DllImport("kernel32.dll")]
            public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

            [DllImport("User32.dll")]
            public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

            [DllImport("User32.dll")]
            public static extern short GetAsyncKeyState(System.Int32 vKey);

            [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern long GetAsyncKeyState(long vKey);

            public static string CutString(string mystring)
            {
                try
                {
                    char[] chArray = mystring.ToCharArray();
                    string str = "";
                    for (int i = 0; i < mystring.Length; i++)
                    {
                        if ((chArray[i] == ' ') && (chArray[i + 1] == ' '))
                        {
                            return str;
                        }
                        if (chArray[i] == '\0')
                        {
                            return str;
                        }
                        str = str + chArray[i].ToString();
                    }
                }
                catch { }
                return mystring.TrimEnd(new char[] { '0' });

            }
        }

        internal class Proc
        {
            public static string Name = "hl";
            public static IntPtr Client;
            public static IntPtr HW;
            public static Process Process { get; set; }

            public static string[] Modules = new string[]
            {
                "client.dll",
                "hw.dll"
            };
        }
    }
}
