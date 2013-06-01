using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKUpdateService.Versions
{
    public class UpdateFile
    {
        public Version Version { get; set; }
        public Uri Url { get; set; }
        public string Hash { get; set; }

        public UpdateFile(Version version, Uri url)
        {
            Version = version;
            Url = url;
        }
    }
}
