using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Backup
{
    class CopyThread
    {
        private volatile Boolean bIsRunning;
        private volatile Boolean bInterrupt;
        private ArrayList sourceDirList;
        private String targetDir;
        private Backup.Form1 form;
        private RichTextBox textBoxOutput;

        public CopyThread()
        {
            this.bInterrupt = false;
            this.bIsRunning = false;
        }

        public void Init(ArrayList sourceDirList, String targetDir, Backup.Form1 form, RichTextBox textBoxOutput)
        {
            this.sourceDirList = new ArrayList(sourceDirList);
            this.targetDir = targetDir;
            this.form = form;
            this.textBoxOutput = textBoxOutput;
        }

        public void StopThread(){
            if (bIsRunning)
            {
                this.bInterrupt = true;
            }
        }

        public Boolean IsRunning()
        {
            return this.bIsRunning;
        }

        public void WriteOutput(int level, String text)
        {
            Debug.WriteLine(text);
            form.Output(level, text);
        }


        public void CopyFiles()
        {
            try
            {
                this.bInterrupt = false;
                this.bIsRunning = true;
                Boolean bWithError = false;

                foreach (Object item in this.sourceDirList)
                {
                    string path = (string)item;       //Pfad wo sich die Datei befindet
                    try
                    {
                        DirectoryInfo rootTargetDirInfo = new DirectoryInfo(targetDir);
                        DirectoryInfo sourceDirInfo = new DirectoryInfo(path);
                        FileInfo sourceFile = new FileInfo(path);
                        if (sourceDirInfo.Exists)
                        {
                            String fullPath = sourceDirInfo.FullName;
                            string[] pathArray = fullPath.Split(new char[] { '\\' });
                            StringBuilder pathAppend = new StringBuilder();
                            pathAppend.Append(rootTargetDirInfo);
                            for (int i = 1; i < pathArray.Length; i++)
                            {
                                pathAppend.Append("\\" + pathArray[i]);
                            }
                            Boolean bResult = this.DeepCopyFiles(sourceDirInfo, pathAppend.ToString());
                            bWithError = bWithError || bResult;
                        }
                        else
                        {
                            this.WriteOutput(Form1.LEVEL_ERROR, "Verzeichnis " + path + " existiert nicht!");
                        }
                        //Kopieren der Datei.
                        //System.IO.File.Copy(path, path2);
                        //this.WriteOutput(Form1.LEVEL_ERROR path + " kopiert!");
                    }
                    catch (Exception ex)
                    {
                        bWithError = true;
                        this.WriteOutput(Form1.LEVEL_ERROR,  path + " kopieren fehlgeschlagen!");
                        this.WriteOutput(Form1.LEVEL_ERROR, ex.Message + "\n");
                    }
                }
                if (this.bInterrupt)
                {
                    this.WriteOutput(Form1.LEVEL_WARN, "Backup abgebrochen!");
                }
                else if (bWithError)
                {
                    this.WriteOutput(Form1.LEVEL_ERROR, "Backup mit Fehler abgeschlossen!");
                }
                else 
                {
                    this.WriteOutput(Form1.LEVEL_SYSTEM, "Backup erfolgreich beendet!");
                }
            }
            catch (Exception exept)
            {
                this.WriteOutput(Form1.LEVEL_ERROR, exept.Message);
            }
            finally
            {
                this.bIsRunning = false;
                this.bInterrupt = false;
                this.form.NotifyFinished();
            }
        }


        private Boolean DeepCopyFiles(DirectoryInfo sourceDir, String targetDirPath)
        {
            Boolean bWithErrors = false;
            if (this.bInterrupt)
            {
                return bWithErrors;
            }
            DirectoryInfo targetDir = new DirectoryInfo(targetDirPath);
            if (!targetDir.Exists)
            {
                targetDir.Create();
            }
            foreach (FileInfo sourceFileInfo in sourceDir.GetFiles())
            {
                if (this.bInterrupt)
                {
                    return bWithErrors;
                }

                try
                {
                    FileInfo targetFileInfo = new FileInfo(targetDir.FullName + "\\" + sourceFileInfo.Name);
                    if (!targetFileInfo.Exists)
                    {
                        this.WriteOutput(Form1.LEVEL_INFO, "Kopiere... " + sourceFileInfo.FullName);
                        System.IO.File.Copy(sourceFileInfo.FullName, targetFileInfo.FullName);

                    }
                    else
                    {
                        // test if file is newer...
                        if (sourceFileInfo.LastWriteTime.ToFileTimeUtc() > targetFileInfo.LastWriteTime.ToFileTimeUtc())
                        {
                            this.WriteOutput(Form1.LEVEL_INFO, "Update... " + targetFileInfo.FullName);

                            FileInfo backupTargetFileInfo = null;
                            FileInfo copyTargetFileInfo = new FileInfo(targetFileInfo.FullName);
                            int idx = 1;
                            do
                            {
                                backupTargetFileInfo = new FileInfo(targetFileInfo.FullName + "_" + idx);
                                idx++;
                            }
                            while (backupTargetFileInfo.Exists);
                            copyTargetFileInfo.MoveTo(backupTargetFileInfo.FullName);
                            System.IO.File.Copy(sourceFileInfo.FullName, targetFileInfo.FullName);
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.WriteOutput(Form1.LEVEL_ERROR, sourceFileInfo.FullName + " kopieren fehlgeschlagen!");
                    this.WriteOutput(Form1.LEVEL_ERROR, ex.Message + "\n");
                    bWithErrors = true;
                }

            }
            foreach (DirectoryInfo sourceDirInfo in sourceDir.GetDirectories())
            {
                if (this.bInterrupt)
                {
                    return bWithErrors;
                }
                DirectoryInfo targetDirInfo = new DirectoryInfo(targetDir.FullName + "\\" + sourceDirInfo.Name);
                Boolean bResult = this.DeepCopyFiles(sourceDirInfo, targetDirInfo.FullName);
                bWithErrors = bWithErrors || bResult;
            }
            return bWithErrors;
        }
    }

}
