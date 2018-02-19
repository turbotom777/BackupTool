using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DateFolder
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2){
                System.Console.WriteLine("Bitte <source pfad> <target pfad> eingeben!");
                return;
            }
            String sourcePath = args[0];
            String targetPath = args[1];
            Program program = new Program();
            program.CopyFiles(sourcePath, targetPath);
        }

        public void WriteOutput(String text)
        {
            System.Console.WriteLine(text);
        }


        public void CopyFiles(String sourcePath, String targetPath)
        {
            try
            {
                DirectoryInfo sourceDirInfo = new DirectoryInfo(sourcePath);
                DirectoryInfo targetDirInfo = new DirectoryInfo(targetPath);
       
                if (!sourceDirInfo.Exists)
                {
                    this.WriteOutput("Verzeichnis " + sourceDirInfo.FullName + " existiert nicht!");
                }

                if (!targetDirInfo.Exists)
                {
                    this.WriteOutput("Verzeichnis " + targetDirInfo.FullName + " existiert nicht!");
                }
                String fullPath = sourceDirInfo.FullName;
                this.WriteOutput("Found " + sourceDirInfo.GetFiles().Length + " files to copy");
                foreach (FileInfo fileInfo in sourceDirInfo.GetFiles())
                {
                    int day = fileInfo.LastWriteTime.ToLocalTime().Day;
                    int month = fileInfo.LastWriteTime.ToLocalTime().Month;
                    int year = fileInfo.LastWriteTime.ToLocalTime().Year;

                    String strDay = day.ToString(); ;
                    if (day < 10)
                    {
                        strDay = "0" + day.ToString();
                    }
                    String strMonth = month.ToString(); ;
                    if (month < 10)
                    {
                        strMonth = "0" + month.ToString();
                    }
                    String path = targetDirInfo.FullName + "\\" + year + "_" + strMonth + "_" + strDay;
                    DirectoryInfo subTargetDirInfo = new DirectoryInfo(path);
                    if (!subTargetDirInfo.Exists)
                    {
                        subTargetDirInfo.Create();
                        this.WriteOutput("Created folder " + subTargetDirInfo.FullName);
                    }
                    String newPath = subTargetDirInfo.FullName + "\\" + fileInfo.Name;

                    fileInfo.MoveTo(newPath);
                }
            }
            catch (Exception ex)
            {
                this.WriteOutput("Kopieren fehlgeschlagen!" + ex.Message);
            }
        }
    }
}
