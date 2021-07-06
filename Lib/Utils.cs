using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Lib
{

    public class Utils
    {
        // https://stackoverflow.com/a/46844044 - had no fucking clue why it doesn't work, my best guess is it wasn't correctly setting the new output stream.
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(string lpFileName
            , [MarshalAs(UnmanagedType.U4)] DesiredAccess dwDesiredAccess
            , [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode
            , uint lpSecurityAttributes
            , [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition
            , [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes
            , uint hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetStdHandle(StdHandle nStdHandle, IntPtr hHandle);

        private enum StdHandle : int
        {
            Input = -10,
            Output = -11,
            Error = -12
        }

        [Flags]
        enum DesiredAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000
        }

        public static void CreateConsole()
        {
            if (AllocConsole())
            {
                var stdOutHandle = CreateFile("CONOUT$", DesiredAccess.GenericRead | DesiredAccess.GenericWrite, FileShare.ReadWrite, 0, FileMode.Open, FileAttributes.Normal, 0);

                if (stdOutHandle == new IntPtr(-1))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                if (!SetStdHandle(StdHandle.Output, stdOutHandle))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                
                // output
                
                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
                
                // error
                
                var standardErr = new StreamWriter(Console.OpenStandardError());
                standardErr.AutoFlush = true;
                Console.SetError(standardErr);
                
            }
        }
    }
}