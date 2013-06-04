using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using MKUpdateService.Tools;
using MKUpdateService.Update;

namespace MKUpdateService
{
    class Program
    {

        public static string OwnerPath;
        public static Uri UrlToVersions;
        public static Version CurrentVersion;
        
        
        static void Main(string[] args)
        {
            new Thread(track).Start();

            if (args.Length != 3)
            {
                Console.WriteLine("Usage: MKUpdateService.exe <C:/path/appToUpdate.exe> <url to text file Versions> <current version>");
                Console.WriteLine("Args count: " + args.Length);
                startApp();
                return;
            }

            OwnerPath = args[0];
            string urlToVersions = args[1];
            string currentVersion = args[2];


            if (!Path.IsPathRooted(OwnerPath))
            {
                Console.WriteLine("First parameter: bad path");
                startApp();
                return;
            }


            UrlToVersions = UsefulFunctions.StringToUri(urlToVersions);
            if (UrlToVersions == null)
            {
                Console.WriteLine("Second parameter: bad url");
                startApp();
                return;
            }

            CurrentVersion = UsefulFunctions.StringToVersion(currentVersion);
            if (CurrentVersion == null)
            {
                Console.WriteLine("Third parameter: bad version format \n Use: 1.0.0.0");
                startApp();
                return;
            }

            var updateMan = new UpdateManager(OwnerPath, UrlToVersions, CurrentVersion);

            updateMan.End += OnEnd;   
            updateMan.Failure += (sender, eventArgs) => Console.WriteLine("[!] " + eventArgs.GetException().ToString());
            updateMan.Information += (sender, eventArgs) => Console.WriteLine("[*] " + eventArgs.Msg);

            updateMan.Start();
        }



        private static void OnEnd(object sender, EventArgs eventArgs)
        {
            startApp();
        }


        private static void startApp()
        {
            Console.WriteLine("[*] Updating was ended");
            if (Path.GetExtension(OwnerPath) == ".exe")
            {
                Console.WriteLine("[+] Starting app: " + OwnerPath);
                try
                {
                    Process.Start(OwnerPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[!] Problem with starting app. Bad path.");
                }
            }
            else
            {
                Console.WriteLine("[!] Cannot start app. First parameter must be executable file.");
            }
        }

        private static void track()
        {
            try
            {
                string url = ConfigurationManager.AppSettings["urlToTracker"];
                if (!String.IsNullOrEmpty(url))
                {
                    HttpWebRequest URLReq = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse URLRes = (HttpWebResponse)URLReq.GetResponse();
                    if (URLRes.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("[*] Tracked");
                    }
                    else
                    {
                        Console.WriteLine("[-] Problem with tracking");
                    }
                }
            }
            catch
            {
                // Nothing
            }
        }
    }
}
