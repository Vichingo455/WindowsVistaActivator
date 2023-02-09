using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static VistaActivator.Program;
using System.Management;

namespace VistaActivator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (IsWindowsUEFI())
            {
                label2.Text = "Firmware: UEFI";
            }
            else
            {
                label2.Text = "Firmware: BIOS";
            }
            //space
            if (GetWindowsEdition() != "Ultimate" && GetWindowsEdition() != "Business" && GetWindowsEdition() != "Enterprise" && GetWindowsEdition() != "HomeBasic" && GetWindowsEdition() != "HomePremium")
            {
                //unsupported Windows Vista Edition
                label1.Text = $@"Windows Edition: Windows Vista {GetWindowsEdition()} Edition";
                label3.Text = "Status: Unsupported Windows Vista Edition";
                button1.Enabled= false;
                button2.Enabled= false;
            }
            else if (IsWindowsUEFI() && GetWindowsEdition() != "Business" && GetWindowsEdition() != "Enterprise")
            {
                //unsupported Windows Vista UEFI Edition
                label1.Text = $@"Windows Edition: Windows Vista {GetWindowsEdition()} Edition";
                label3.Text = "Status: Unsupported partition table";
                button1.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                label1.Text = $@"Windows Edition: Windows Vista {GetWindowsEdition()} Edition";
                label3.Text = "Status: Ready";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            button3.Visible = false;
            Thread thr = new Thread(InstallThread);
            thr.Start();
        }
        private void InstallThread()
        {
            this.Enabled = false;
            try
            {
                if (Directory.Exists(TempActivatorPath))
                {
                    Directory.Delete(TempActivatorPath, true);
                }
                if (GetWindowsEdition() == "Enterprise")
                {
                    HiddenProcess.StartWaitHiddenProcess("cscript.exe",$@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ipk VKK3X-68KWM-X2YGT-QR4M6-4BWMV","",true);
                    progressBar1.Value = 30;
                    HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -skms kms9.msguides.com", "", true);
                    progressBar1.Value = 50;
                    HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ato", "", true);
                    progressBar1.Value = 100;
                }
                else if (GetWindowsEdition() == "Business")
                {
                    HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ipk YFKBB-PQJJV-G996G-VWGXY-2V3X8", "", true);
                    progressBar1.Value = 30;
                    HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -skms kms9.msguides.com", "", true);
                    progressBar1.Value = 50;
                    HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ato", "", true);
                    progressBar1.Value = 100;
                }
                else
                {
                    Directory.CreateDirectory(TempActivatorPath);
                    File.WriteAllBytes(TempActivatorPath + @"\Certificate.xrm-ms", Properties.Resources.Certificate);
                    File.WriteAllBytes(TempActivatorPath + @"\bootinst.exe", Properties.Resources.bootinst);
                    File.WriteAllBytes(TempActivatorPath + @"\grldr", Properties.Resources.grldr);
                    HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ilc %temp%\WinVistaActivator.tmp\Certificate.xrm-ms", "", true);
                    progressBar1.Value = 30;
                    File.Copy(TempActivatorPath + @"\grldr",GetWindowsDrive + @"\grldr",true);
                    HiddenProcess.StartWaitHiddenProcess(TempActivatorPath + @"\bootinst.exe",$"/nt60 {GetWindowsDrive}","",true);
                    HiddenProcess.StartWaitHiddenProcess("cmd.exe",$@"/c attrib +s +h +i +r {GetWindowsDrive}\grldr && exit","",true);
                    progressBar1.Value = 50;
                    if (GetWindowsEdition() == "Ultimate")
                    {
                        HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ipk 6F2D7-2PCG6-YQQTB-FWK9V-932CC", "", true);
                        HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ato", "", true);
                        progressBar1.Value = 100;
                    }
                    else if (GetWindowsEdition() == "HomePremium")
                    {
                        HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ipk 8XPM9-7F9HD-4JJQP-TP64Y-RPFFV", "", true);
                        HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ato", "", true);
                        progressBar1.Value = 100;
                    }
                    else if (GetWindowsEdition() == "HomeBasic")
                    {
                        HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ipk 762HW-QD98X-TQVXJ-8RKRQ-RJC9V", "", true);
                        HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ato", "", true);
                        progressBar1.Value = 100;
                    }
                }
                var dlg = MessageBox.Show("A restart is required to successfully activate Windows. Would you like to restart?","Windows Vista Activator for BIOS & UEFI",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dlg == DialogResult.Yes) 
                {
                    string computerName = Environment.MachineName.ToString(); // computer name or IP address

                    ConnectionOptions options = new ConnectionOptions();
                    options.EnablePrivileges = true;
                    // To connect to the remote computer using a different account, specify these values:
                    // options.Username = "USERNAME";
                    // options.Password = "PASSWORD";
                    // options.Authority = "ntlmdomain:DOMAIN";

                    ManagementScope scope = new ManagementScope(
                      "\\\\" + computerName + "\\root\\CIMV2", options);
                    scope.Connect();

                    SelectQuery query = new SelectQuery("Win32_OperatingSystem");
                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher(scope, query);

                    foreach (ManagementObject os in searcher.Get())
                    {
                        // Obtain in-parameters for the method
                        ManagementBaseObject inParams =
                            os.GetMethodParameters("Win32Shutdown");

                        // Add the input parameters.
                        inParams["Flags"] = 2;

                        // Execute the method and obtain the return values.
                        ManagementBaseObject outParams =
                            os.InvokeMethod("Win32Shutdown", inParams, null);
                    }
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                MessageBox.Show($"Error: {ex.Message}. You can ask for support","Windows Vista Activator for UEFI & BIOS",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            button3.Visible = false;
            Thread thr = new Thread(UninstallThread);
            thr.Start();
        }
        private void UninstallThread()
        {
            this.Enabled = false;
            try
            {
                HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -ckms", "", true);
                progressBar1.Value = 25;
                HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -upk", "", true);
                progressBar1.Value = 50;
                HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -cpky", "", true);
                progressBar1.Value = 75;
                HiddenProcess.StartWaitHiddenProcess("cscript.exe", $@"{GetWindowsDrive}\Windows\System32\slmgr.vbs -rilc", "", true);
                progressBar1.Value = 100;
                var dlg = MessageBox.Show("A restart is required to successfully uninstall the tool. Would you like to restart?", "Windows Vista Activator for BIOS & UEFI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg == DialogResult.Yes)
                {
                    string computerName = Environment.MachineName.ToString(); // computer name or IP address

                    ConnectionOptions options = new ConnectionOptions();
                    options.EnablePrivileges = true;
                    // To connect to the remote computer using a different account, specify these values:
                    // options.Username = "USERNAME";
                    // options.Password = "PASSWORD";
                    // options.Authority = "ntlmdomain:DOMAIN";

                    ManagementScope scope = new ManagementScope(
                      "\\\\" + computerName + "\\root\\CIMV2", options);
                    scope.Connect();

                    SelectQuery query = new SelectQuery("Win32_OperatingSystem");
                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher(scope, query);

                    foreach (ManagementObject os in searcher.Get())
                    {
                        // Obtain in-parameters for the method
                        ManagementBaseObject inParams =
                            os.GetMethodParameters("Win32Shutdown");

                        // Add the input parameters.
                        inParams["Flags"] = 2;

                        // Execute the method and obtain the return values.
                        ManagementBaseObject outParams =
                            os.InvokeMethod("Win32Shutdown", inParams, null);
                    }
                }
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                MessageBox.Show($"Error: {ex.Message}. You can ask for support", "Windows Vista Activator for UEFI & BIOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Windows_Vista_OEM_PreActivation.zip"))
                {
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Windows_Vista_OEM_PreActivation.zip");
                }
                File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Windows_Vista_OEM_PreActivation.zip",Properties.Resources.OEMPreActivation);
                MessageBox.Show($"Done, extracted the zip file to {Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Windows_Vista_OEM_PreActivation.zip"}","Windows Vista Activator for UEFI & BIOS",MessageBoxButtons.OK,MessageBoxIcon.Information);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                MessageBox.Show($"Error: {ex.Message}. You can ask for support", "Windows Vista Activator for UEFI & BIOS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
