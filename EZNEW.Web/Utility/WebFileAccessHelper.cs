using EZNEW.Framework.IoC;
using EZNEW.Web.Config;
using EZNEW.Web.Config.FileAccess;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// Web File Access
    /// </summary>
    public static class WebFileAccessHelper
    {
        /// <summary>
        /// get file full path
        /// </summary>
        public static string GetFileFullPath(string key, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return string.Empty;
            }
            var fileConfig = ContainerManager.Resolve<IOptions<FileAccessPathConfig>>()?.Value;
            if (fileConfig == null)
            {
                return filePath;
            }
            return fileConfig.GetRandomFileFullPath(key, filePath);
        }
    }
}
