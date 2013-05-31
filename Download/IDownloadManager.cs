using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKUpdateService.Download
{
    public interface IDownloadManager
    {
        event System.Net.DownloadProgressChangedEventHandler DownloadProgressChanged;

        event System.ComponentModel.AsyncCompletedEventHandler DownloadCompleted;

        string PathToUpdateDirectory { get; set; }
        
        void DownloadFile(Uri pathToFile);

        void DownloadFiles(Uri[] pathsToFiles);
    }
}
