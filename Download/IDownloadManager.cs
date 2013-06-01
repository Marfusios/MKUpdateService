using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MKUpdateService.Versions;

namespace MKUpdateService.Download
{
    public interface IDownloadManager
    {
        event System.Net.DownloadProgressChangedEventHandler DownloadProgressChanged;

        event DownloadManager.DownloadedUpdateFileEventHandler DownloadCompleted;

        string Destination { get; set; }
        
        void DownloadFile(Uri pathToFile);

        void DownloadFile(UpdateFile pathToFile);

        void DownloadFiles(Uri[] pathsToFiles);

        void DownloadFiles(List<Uri> pathsToFiles);

        void DownloadFiles(List<UpdateFile> pathsToFiles);
    }
}
