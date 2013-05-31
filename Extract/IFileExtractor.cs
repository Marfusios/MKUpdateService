using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKUpdateService.Extract
{
    interface IFileExtractor
    {
        string Destination { get; set; }

        void Extract(string filePath);
    }
}
