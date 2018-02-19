using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Collections;

namespace Backup
{
    public partial class Form1 : Form
    {
        CopyThread copyThread = new CopyThread();

        public static int LEVEL_SYSTEM = 0;
        public static int LEVEL_INFO = 1;
        public static int LEVEL_WARN = 2;
        public static int LEVEL_ERROR = 3;

        public Form1()
        {
            InitializeComponent();
            ReadRegistry();
            this.CenterToScreen();
            this.buttonCancel.Enabled = false;

        }

        public delegate void WriteOutputDelegate(int level, String text);
        public delegate void EnableButtonDelegate(Button button, Boolean bEnable);

        protected void ReadRegistry()
        {            // read dirs from registry
            RegistryKey localMachineKey = Registry.LocalMachine;
            RegistryKey softwareKey = localMachineKey.OpenSubKey("SOFTWARE", true);
            RegistryKey tomsKey = softwareKey.OpenSubKey("toms", true);
            if (tomsKey == null)
            {
                softwareKey.CreateSubKey(@"toms\target");
                softwareKey.CreateSubKey(@"toms\source");
            }
            else
            {
                RegistryKey targetDirKey = tomsKey.OpenSubKey("target", false);
                if (targetDirKey != null)
                {
                    this.textBoxTargetDir.Text = (string)targetDirKey.GetValue("dir");
                }
                RegistryKey sourceDirKey = tomsKey.OpenSubKey("source", false);
                if (sourceDirKey != null)
                {
                    String[] keyNames = sourceDirKey.GetValueNames();
                    for (int i = 0; i < keyNames.Length; i++)
                    {
                        String key = keyNames[i];
                        String value = (string)sourceDirKey.GetValue(key);
                        this.listBoxSourceDir.Items.Add(value);
                    }
                }
            }
            localMachineKey.Close();
        }

        protected void WriteRegistry()
        {
            RegistryKey localMachineKey = Registry.LocalMachine;
            RegistryKey softwareKey = localMachineKey.OpenSubKey("SOFTWARE", true);
            RegistryKey tomsKey = softwareKey.OpenSubKey("toms", true);
            if (tomsKey == null)
            {
                softwareKey.CreateSubKey(@"toms\target");
                softwareKey.CreateSubKey(@"toms\source");
            }

            RegistryKey targetDirKey = tomsKey.OpenSubKey("target", true);
            if (targetDirKey == null)
            {
                tomsKey.CreateSubKey("target");
            }
            targetDirKey.SetValue("dir", this.textBoxTargetDir.Text);

            RegistryKey sourceDirKey = tomsKey.OpenSubKey("source", true);
            if (sourceDirKey == null)
            {
                tomsKey.CreateSubKey("source");
            }

            String[] keyNames = sourceDirKey.GetValueNames();
            for (int i = 0; i < keyNames.Length; i++)
            {
                String key = keyNames[i];
                sourceDirKey.DeleteValue(key);
            }
            int n = 1;
            foreach (Object item in this.listBoxSourceDir.Items)
            {
                String value = (string)item;
                sourceDirKey.SetValue("dir" + n, value);
                n++;
            }
            localMachineKey.Close();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }

        private void buttonSourceSelect_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialogSource.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                if (!this.listBoxSourceDir.Items.Contains(this.folderBrowserDialogSource.SelectedPath)){
                    this.listBoxSourceDir.Items.Add(this.folderBrowserDialogSource.SelectedPath);
                } else 
                {
                    this.WriteOutput(Form1.LEVEL_WARN, "Verzeichnis ist bereits in der Liste vorhanden!");
                }
            }
        }

        private void buttonTargetSelect_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialogTarget.ShowDialog();
            if (result.Equals(DialogResult.OK))
            {
                this.textBoxTargetDir.Text = this.folderBrowserDialogTarget.SelectedPath;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.copyThread.IsRunning())
            {
                return;
            }

            Boolean bValidSelection = true;
            if (this.listBoxSourceDir.Items.Count <= 0)
            {
                this.WriteOutput(Form1.LEVEL_WARN, "Kein Verzeichnis zum kopieren ausgewählt!");
                bValidSelection = false;
            }

            if (this.textBoxTargetDir.Text == null || this.textBoxTargetDir.Text.Trim().Length <= 0)
            {
                this.WriteOutput(Form1.LEVEL_WARN, "Kein Zielverzeichnis ausgewählt!");
                bValidSelection = false;
            }
            else
            {
                DirectoryInfo rootTargetDirInfo = new DirectoryInfo(textBoxTargetDir.Text);
                if (!rootTargetDirInfo.Exists)
                {
                    this.WriteOutput(Form1.LEVEL_ERROR, "Ungültiges Zielverzeichnis oder Laufwerk: '" + rootTargetDirInfo.FullName + "'!");
                    bValidSelection = false;
                }
            }

            if (bValidSelection)
            {
                this.buttonCancel.Enabled = true;
                this.buttonStart.Enabled = false;
                this.buttonClose.Enabled = false;

                Thread workerThread = new Thread(copyThread.CopyFiles);
                ArrayList list = new ArrayList();
                foreach (Object item in this.listBoxSourceDir.Items)
                {
                    list.Add(item);       //Pfad wo sich die Datei befindet

                }

                
                copyThread.Init(list, this.textBoxTargetDir.Text, this, this.textBoxOutput);
                // Start the worker thread.
                workerThread.Start();
            }
        }

        private void WriteOutput(int level, String text)
        {
            if (level == Form1.LEVEL_ERROR)
            {
                this.textBoxOutput.SelectionBackColor = System.Drawing.Color.LightSalmon;
            }
            if (level == Form1.LEVEL_WARN)
            {
                this.textBoxOutput.SelectionBackColor = System.Drawing.Color.LightBlue;
            }
            if (level == Form1.LEVEL_INFO)
            {
                this.textBoxOutput.SelectionBackColor = System.Drawing.Color.White;
            }
            if (level == Form1.LEVEL_SYSTEM)
            {
                this.textBoxOutput.SelectionBackColor = System.Drawing.Color.Yellow;
            }
            this.textBoxOutput.AppendText(text + "\n");
            this.textBoxOutput.ScrollToCaret();
        }

        public void Output(int level, String text)
        {
            WriteOutputDelegate outputDelegate = new WriteOutputDelegate(WriteOutput);
            this.textBoxOutput.Invoke(outputDelegate, new Object[] { level, text});
        }

        public void NotifyFinished()
        {
            EnableButtonDelegate enableButtonDelegate = new EnableButtonDelegate(ResetButton);
            this.buttonCancel.Invoke(enableButtonDelegate, new Object[] { this.buttonCancel, false });
            this.buttonStart.Invoke(enableButtonDelegate, new Object[] { this.buttonStart, true });
            this.buttonClose.Invoke(enableButtonDelegate, new Object[] { this.buttonClose, true });
        }
        private void ResetButton(Button button, Boolean bEnable)
        {
            button.Enabled = bEnable;
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            //this.buttonStart.Enabled = true;
            //this.buttonCancel.Enabled = false;
            this.copyThread.StopThread();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.textBoxOutput.Clear();
        }

        private void buttonSourceRemove_Click(object sender, EventArgs e)
        {
            int selectedIndex = this.listBoxSourceDir.SelectedIndex;
            if (selectedIndex >= 0)
            {
                this.listBoxSourceDir.Items.RemoveAt(selectedIndex);
            }
            else
            {
                this.WriteOutput(Form1.LEVEL_WARN, "Kein Verzeichnis zum Entfernen ausgewählt!");
            }
        }
    }
}