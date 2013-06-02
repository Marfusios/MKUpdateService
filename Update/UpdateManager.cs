using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using MKUpdateService.Download;
using MKUpdateService.Tools;
using MKUpdateService.Versions;

namespace MKUpdateService.Update
{
    public class UpdateManager
    {

        #region EVENTS

        public event ErrorEventHandler Failure;
        protected virtual void OnFailure(ErrorEventArgs e)
        {
            ErrorEventHandler handler = Failure;
            if (handler != null) 
                handler(this, e);
        }

        public event EventHandler End;
        protected virtual void OnEnd()
        {
            EventHandler handler = End;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public delegate void InformationEventHandler(object sender, InformationEventArgs e);
        public event InformationEventHandler Information;
        protected virtual void OnInformation(InformationEventArgs e)
        {
            InformationEventHandler handler = Information;
            if (handler != null) handler(this, e);
        }

        #endregion


        private string ownerOrDirectoryPath;
        public string OwnerOrDirectoryPath 
        { 
            get { return ownerOrDirectoryPath; }
            set
            {
                if (Path.IsPathRooted(value))
                {
                    if(Path.HasExtension(value))
                        ownerOrDirectoryPath = Path.GetDirectoryName(value);
                    else
                    {
                        ownerOrDirectoryPath = value;
                    }
                }
                else
                {
                    ownerOrDirectoryPath = String.Empty;
                }
            }
        }
        
        public Uri VersionsFile { get; set; }
        public Version CurrentVersion { get; set; }


        public UpdateManager(string ownerOrDirectoryPath, Uri versionsFile, Version currentVersion)
        {
            OwnerOrDirectoryPath = ownerOrDirectoryPath;
            VersionsFile = versionsFile;
            CurrentVersion = currentVersion;
        }

        /// <summary>
        /// Start updating. Use events to get informations.
        /// </summary>
        public void Start()
        {
            try
            {
                IVersionParser verPar;
                if(Path.GetExtension(VersionsFile.ToString()) == ".txt")
                    verPar = new VersionParserTxt();
                /*else if (Path.GetExtension(VersionsFile.ToString()) == ".xml")
                {
                     verPar = new VersionParserXml();
                }*/
                else throw new InvalidOperationException("Cannot parse this type of version file.");
                
                    
                List<UpdateFile> availableVersions = verPar.GetMissingVersions(VersionsFile, CurrentVersion);

                foreach (var ver in availableVersions)
                {
                    OnInformation(new InformationEventArgs("Available version: " + ver.Version.ToString()));
                }

                List<string> downloadedFiles = new List<string>();

                IDownloadManager dowMan = new DownloadManager() {Destination = OwnerOrDirectoryPath};
                dowMan.DownloadCompleted += (sender, args) => OnInformation(new InformationEventArgs("Downloaded and saved to: " + args.PathToFileOnComputer));
                dowMan.DownloadCompleted += (sender, args) => downloadedFiles.Add(args.PathToFileOnComputer);
                dowMan.DownloadFiles(availableVersions);

                while (availableVersions.Count != downloadedFiles.Count)
                {
                    Thread.Sleep(250);
                }

                downloadedFiles.Sort();

                var extractor = new Extract.ExtractorZip() {Destination = OwnerOrDirectoryPath};

                foreach (var downloadedFile in downloadedFiles)
                {
                    extractor.Extract(downloadedFile);
                    OnInformation(new InformationEventArgs("Extracted file: " + Path.GetFileName(downloadedFile)));
                }


            }
            catch (Exception e)
            {
                OnFailure(new ErrorEventArgs(e));
            }
            finally
            {
                removeTemporaryFolder();
                OnEnd();
            }
        }

        private void removeTemporaryFolder()
        {
            if (Directory.Exists(OwnerOrDirectoryPath + Configuration.UpdateDirectory))
            {
                try
                {
                    Directory.Delete(OwnerOrDirectoryPath + Configuration.UpdateDirectory, true);
                    OnInformation(new InformationEventArgs("Temporary directory was removed."));
                }
                catch (Exception e)
                {
                    OnFailure(new ErrorEventArgs(e));
                }  
            }
        }


    }
}
