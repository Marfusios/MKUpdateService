using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKUpdateService.Versions
{
    interface IVersionParser
    {
        List<UpdateFile> GetMissingVersions(Uri uriToVersions, Version currentVersion);
    }
}
