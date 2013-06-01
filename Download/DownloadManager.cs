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
        #region EVENTS

        public event DownloadProgressChangedEventHandler DownloadProgressChanged;

        public delegate void DownloadedUpdateFileEventHandler(object sender, DownloadedUpdateFileEventArgs e);

        public event DownloadedUpdateFileEventHandler DownloadCompleted;
        protected virtual void OnDownloadCompleted(DownloadedUpdateFileEventArgs downloadedUpdateFileEventArgs)
        {
            DownloadedUpdateFileEventHandler handler = DownloadCompleted;
            if (handler != null) handler(this, downloadedUpdateFileEventArgs);
        }

        #endregion


        private WebClient webClient; // = new WebClient();

        public string Destination { get; set; }

        public void DownloadFile(Uri pathToFile)
        {
            try
            {
                this.webClient = new WebClient();       

                if (string.IsNullOrEmpty(Destination))
                {
                    Destination = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Configuration.UpdateDirectory;
                }
                else if (!Destination.Contains(Configuration.UpdateDirectory))
                {
                    Destination += Configuration.UpdateDirectory;
                }


                if (!Directory.Exists(Destination))
                {
                    Directory.CreateDirectory(Destination);
                }


                // Download
                string pathToFileOnComputer = Destination + @"\\" +
                                              Path.GetFileName(pathToFile.ToString());

                this.webClient.DownloadProgressChanged += DownloadProgressChanged;
                this.webClient.DownloadFileCompleted +=
                    (sender, args) => OnDownloadCompleted(new DownloadedUpdateFileEventArgs(pathToFileOnComputer));

                this.webClient.DownloadFileAsync(pathToFile, pathToFileOnComputer);

            }
            catch (Exception e)
            {
                throw new InvalidOperationException("An error occurred while trying to download update file from " + pathToFile + " : ", e);
            }
        }

        public void DownloadFile(Versions.UpdateFile updateFile)
        {
            DownloadFile(updateFile.Url);
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

        public void DownloadFiles(List<Uri> pathsToFiles)
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
        
        public void DownloadFiles(List<Versions.UpdateFile> updateFiles)
        {
            try
            {
                foreach (var uF in updateFiles)
                {
                    DownloadFile(uF);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot download one file, downloading was interrupt: " + e);
            }
        }
    }
}
