using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VistaActivator
{
    internal static class Program
    {
        /// <summary>
        /// Get Windows Drive (C:\)
        /// </summary>
        public static string GetWindowsDrive = Path.GetPathRoot(Environment.SystemDirectory);
        /// <summary>
        /// 
        /// </summary>
        public static string TempActivatorPath = Path.GetTempPath() + @"\WinVistaActivator.tmp";
        public const int ERROR_INVALID_FUNCTION = 1;

        [DllImport("kernel32.dll", EntryPoint = "GetFirmwareEnvironmentVariableW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetFirmwareType(string lpName, string lpGUID, IntPtr pBuffer, uint size);

        /// <summary>
        /// Returns if Windows is installed on a UEFI or BIOS firmware
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsUEFI()
        {
            // Call the function with a dummy variable name and a dummy variable namespace (function will fail because these don't exist.)
            GetFirmwareType("", "{00000000-0000-0000-0000-000000000000}", IntPtr.Zero, 0);

            if (Marshal.GetLastWin32Error() == ERROR_INVALID_FUNCTION)
            {
                // Calling the function threw an ERROR_INVALID_FUNCTION win32 error, which gets thrown if either
                // - The mainboard doesn't support UEFI and/or
                // - Windows is installed in legacy BIOS mode
                return false;
            }
            else
            {
                // If the system supports UEFI and Windows is installed in UEFI mode it doesn't throw the above error, but a more specific UEFI error
                return true;
            }
        }
        private static string returnvalue; //GetWindowsEdition return value
        /// <summary>
        /// Returns the current Windows Edition
        /// </summary>
        /// <returns></returns>
        public static string GetWindowsEdition()
        {
            
            try
            {
                RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                returnvalue = (string)key.GetValue("EditionID");
                key.Close();
            }
            catch 
            { returnvalue = "Unknown"; }
            return returnvalue;
        }
        /// <summary>
        /// Checks if the system is Windows Vista
        /// </summary>
        /// <returns></returns>
        public static bool IsWindowsVista()
        {
            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 0)
            {
                return true;
            }
            else { return false; }
        }
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (IsWindowsVista()) 
            {
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("This program works only on Windows Vista!","Windows Vista Activator for BIOS & UEFI",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }
    }
}
