using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKUpdateService.Download
{
    public class DownloadedUpdateFileEventArgs : EventArgs
    {
        public string PathToFileOnComputer { get; set; }

        public DownloadedUpdateFileEventArgs(string pathToFileOnComputer)
        {
            PathToFileOnComputer = pathToFileOnComputer;
        }
    }
}
