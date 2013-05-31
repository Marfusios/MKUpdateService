using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using MKUpdateService.Tools;

namespace MKUpdateService.Extract
{
    class ExtractorZip : IFileExtractor
    {
        public string Destination { get; set; }


        public void Extract(string filePath)
        {
            try
            {
                using (ZipFile zip1 = ZipFile.Read(filePath))
                {

                    string location = handleDestination(filePath);

                    foreach (ZipEntry e in zip1)
                    {
                        string tmpFileName = Path.Combine(location, e.FileName + ".tmp");
                        string existingFileName = Path.Combine(location, e.FileName);
                        string pendingFileName = Path.Combine(location, e.FileName + ".exe.PendingOverwrite");

                        if (File.Exists(pendingFileName))
                        {
                            File.SetAttributes(existingFileName, FileAttributes.Normal);
                            File.Delete(pendingFileName);
                        }

                        if (File.Exists(existingFileName))
                        {
                            File.SetAttributes(existingFileName, FileAttributes.Normal);
                            File.Delete(existingFileName);
                        }

                        if (File.Exists(tmpFileName))
                        {
                            File.SetAttributes(tmpFileName, FileAttributes.Normal);
                            File.Delete(tmpFileName);
                        }


                        e.Extract(location, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error while extracting file (perhaps you did't specify zip file? Or bad destination path) ", e);
            }
        }

        private string handleDestination(string filePath)
        {
            if (string.IsNullOrEmpty(Destination))
            {
                string dir = Path.GetDirectoryName(filePath);

                if (dir != null && dir.Contains(Configuration.UpdateDirectory))
                {
                    return dir.Replace(Configuration.UpdateDirectory, "");
                }
                
                if (dir == null) throw new FormatException("Cannot find directory of extracting file");
                else return dir;
            }
            else
            {
                return Destination;
            }
        }
    }
}
