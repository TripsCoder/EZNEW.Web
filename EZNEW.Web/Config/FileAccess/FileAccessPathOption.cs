using EZNEW.Framework.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EZNEW.Web.Config.FileAccess
{
    /// <summary>
    /// File Access Path Option
    /// </summary>
    public class FileAccessPathOption
    {
        /// <summary>
        /// root paths
        /// </summary>
        public List<string> RootPaths
        {
            get; set;
        }

        /// <summary>
        /// get file access path with random
        /// </summary>
        /// <param name="filePath">relative file path</param>
        /// <returns></returns>
        public string GetRandomFileFullPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || RootPaths.IsNullOrEmpty())
            {
                return string.Empty;
            }
            string rootPath = string.Empty;
            int rootCount = RootPaths.Count;
            if (rootCount == 1)
            {
                rootPath = RootPaths[0];
            }
            else
            {
                Random random = new Random();
                int ranIndex = random.Next(0, rootCount);
                rootPath = RootPaths[ranIndex];
            }
            string fullPath = Path.Combine(rootPath, filePath);
            fullPath = fullPath.Replace("\\", "/");
            return fullPath;
        }
    }
}
