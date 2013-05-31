using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MKUpdateService.Download;
using MKUpdateService.Tools;
using MKUpdateService.Versions;

namespace MKUpdateService
{
    class Program
    {

        public static string OwnerPath;
        public static Uri UrlToVersions;
        public static Version CurrentVersion;
        
        
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: MKUpdateService.exe <C:/path/appToUpdate.exe> <url to text file Versions> <current version>");
                return;
            }

            OwnerPath = args[0];
            string urlToVersions = args[1];
            string currentVersion = args[2];


            if (!Path.IsPathRooted(OwnerPath))
            {
                Console.WriteLine("First parameter: bad path");
                return;
            }


            UrlToVersions = UsefulFunctions.StringToUri(urlToVersions);
            if (UrlToVersions == null)
            {
                Console.WriteLine("Second parameter: bad url");
                return;
            }

            CurrentVersion = UsefulFunctions.StringToVersion(currentVersion);
            if (CurrentVersion == null)
            {
                Console.WriteLine("Third parameter: bad version format \n Use: 1.0.0.0");
                return;
            }


            /*var dowMan = new DownloadManager();

            dowMan.DownloadCompleted +=
                (sender, eventArgs) => Console.WriteLine("Downloaded");
            dowMan.DownloadProgressChanged += (sender, eventArgs) => Console.Write(eventArgs.ProgressPercentage + " ");


            dowMan.DownloadFile(new Uri("http://decrypter.9e.cz/updates/update1_4.zip"));
            */
            
            /*IVersionParser parser = new VersionParserTxt();

            var list = parser.GetMissingVersions(UrlToVersions, new Version(1,1));

            foreach (var uri in list)
            {
                Console.WriteLine(uri.ToString());
            }*/

            /*var extr = new Extract.ExtractorZip();

            extr.Destination = @"C:\Users\m4r10\Desktop\test\tmp\moje";
            extr.Extract(@"C:\Users\m4r10\Desktop\test\tmp\hej.zip");*/

           Console.WriteLine("Done");  
           Console.Read();
        }

        
        
    }
}
