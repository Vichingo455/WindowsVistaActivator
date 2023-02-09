using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VistaActivator
{
    internal class HiddenProcess
    {
        /// <summary>
        /// Starts an hidden process
        /// </summary>
        /// <param name="filename">Process file name</param>
        /// <param name="arguments">Arguments (cmd parameters) for the file name</param>
        /// <param name="verbose">Verbose (usually we use "runas" here for triggering uac)</param>
        /// <param name="shellexecute">Launches program from the shell (true or false)</param>
        public static void StartHiddenProcess(string filename, string arguments,string verbose, bool shellexecute)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = filename;
            info.Arguments = arguments;
            info.Verb = verbose;
            info.UseShellExecute = shellexecute;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            Process.Start(info);
            return;
        }
        /// <summary>
        /// Starts an hidden process and waits for its termination
        /// </summary>
        /// <param name="filename">Process file name</param>
        /// <param name="arguments">Arguments (cmd parameters) for the file name</param>
        /// <param name="verbose">Verbose (usually we use "runas" here for triggering uac)</param>
        /// <param name="shellexecute">Launches program from the shell (true or false)</param>
        public static void StartWaitHiddenProcess(string filename, string arguments, string verbose, bool shellexecute)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = filename;
            info.Arguments = arguments;
            info.Verb = verbose;
            info.UseShellExecute = shellexecute;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            Process.Start(info).WaitForExit();
            return;
        }
    }
}
