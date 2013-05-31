using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MKUpdateService.Tools;

namespace MKUpdateService.Download
{
    public class DownloadManager : IDownloadManager
    {
        public event System.Net.DownloadProgressChangedEventHandler DownloadProgressChanged;

        public event System.ComponentModel.AsyncCompletedEventHandler DownloadCompleted;

        private readonly WebClient webClient = new WebClient();

        public string PathToUpdateDirectory { get; set; }

        public void DownloadFile(Uri pathToFile)
        {
            try
            {
                this.webClient.DownloadProgressChanged += DownloadProgressChanged;
                this.webClient.DownloadFileCompleted += DownloadCompleted;

                if (string.IsNullOrEmpty(PathToUpdateDirectory))
                {
                    PathToUpdateDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Configuration.UpdateDirectory;
                }


                if (!Directory.Exists(PathToUpdateDirectory))
                {
                    Directory.CreateDirectory(PathToUpdateDirectory);
                }


                // Download
                string pathToFileOnComputer = PathToUpdateDirectory + @"\\" +
                                              Path.GetFileName(pathToFile.ToString());

                this.webClient.DownloadFileAsync(pathToFile, pathToFileOnComputer);

            }
            catch (Exception e)
            {
                throw new InvalidOperationException("An error occurred while trying to download update file from " + pathToFile + " : ", e);
            }
        }

        public void DownloadFiles(Uri[] pathsToFiles)
        {
            try
            {
                foreach (var path in pathsToFiles)
                {
                    DownloadFile(path);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot download one file, downloading was interrupt: " + e);
            }
        }


        
    }
}
