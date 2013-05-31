using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MKUpdateService.Tools
{
    public class Configuration
    {

        private static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


        public static string UpdateDirectory
        {
            get
            {
                string result = GetAppSetting("updateDirectory");
                if (String.IsNullOrEmpty(result))
                {
                    result = @"\tmp";
                }
                return result;
            }
        }

        public static string UpdateFileExtension
        {
            get
            {
                string result = GetAppSetting("updateFileExtension");
                if (String.IsNullOrEmpty(result))
                {
                    result = ".zip";
                }
                return result;
            }
        }

    }
}
