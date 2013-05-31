using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKUpdateService.Tools
{
    public static class UsefulFunctions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Uri or null</returns>
        public static Uri StringToUri(string url)
        {
            try
            {
                 return new Uri(url);
            }
            catch (UriFormatException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="version"></param>
        /// <returns>Version or null</returns>
        public static Version StringToVersion(string version)
        {
            try
            {
                return new Version(version);
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
