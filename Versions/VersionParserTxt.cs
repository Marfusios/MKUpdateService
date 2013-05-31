using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MKUpdateService.Tools;

namespace MKUpdateService.Versions
{
    class VersionParserTxt : IVersionParser
    {
        private readonly WebClient webClient = new WebClient();
        
       /// <summary>
       /// Method parses text file, each version on one line
       /// EXAMPLE:
       /// 1.0.0.0;http://myweb.com/update1_0.zip
       /// 1.2;http://myweb.com/update1_2.zip
       /// </summary>
       /// <param name="uriToVersions"></param>
       /// <param name="currentVersion"></param>
       /// <returns></returns>
        public List<Uri> GetMissingVersions(Uri uriToVersions, Version currentVersion)
        {
            if(Path.GetExtension(uriToVersions.ToString()) != ".txt") throw new ArgumentException("This parser takes only txt files", "uriToVersions");

            List<Uri> result = new List<Uri>();

            try
            {
                string allText = webClient.DownloadString(uriToVersions);

                string[] lines = allText.Split('\n');

                foreach (var line in lines)
                {
                    string[] items = line.Split(';');

                    if(items.Length < 2) throw new InvalidOperationException("Problem with parsing line: " + line);


                    Version version = Tools.UsefulFunctions.StringToVersion(items[0]);
                    if(version == null) throw new InvalidOperationException("Cannot parse version on line: " + line);

                    if (currentVersion < version)
                    {
                        Uri path = Tools.UsefulFunctions.StringToUri(items[1]);
                        if (path == null) throw new InvalidOperationException("Cannot parse uri on line: " + line);

                        if(Path.GetExtension(path.ToString()) != Configuration.UpdateFileExtension) throw new FormatException("Uri does not contain expected extension, should be: " + Configuration.UpdateFileExtension);

                        result.Add(path);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error while reading versions text file: " + e);
            }
        }


    }
}
