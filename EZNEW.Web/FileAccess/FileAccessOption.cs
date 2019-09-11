using EZNEW.Framework.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EZNEW.Web.FileAccess
{
    /// <summary>
    /// file access path option
    /// </summary>
    public class FileAccessOption
    {
        /// <summary>
        /// root paths
        /// </summary>
        public List<string> RootPaths
        {
            get; set;
        }

        /// <summary>
        /// path pattern
        /// </summary>
        public FilePathPattern PathPattern
        {
            get; set;
        } = FilePathPattern.Random;

        /// <summary>
        /// get file access path with random
        /// </summary>
        /// <param name="fileRelativePath">relative file path</param>
        /// <returns></returns>
        public string GetFilePath(string fileRelativePath)
        {
            if (string.IsNullOrWhiteSpace(fileRelativePath))
            {
                return string.Empty;
            }
            if (RootPaths.IsNullOrEmpty())
            {
                return fileRelativePath;
            }

            #region get root path

            string rootPath = string.Empty;
            int rootCount = RootPaths.Count;
            if (rootCount == 1)
            {
                rootPath = RootPaths[0];
            }
            else
            {
                switch (PathPattern)
                {
                    case FilePathPattern.Random:
                        var random = new Random();
                        int ranIndex = random.Next(0, rootCount);
                        rootPath = RootPaths[ranIndex];
                        break;
                }
            }

            #endregion

            string fullPath = Path.Combine(rootPath, fileRelativePath);
            fullPath = fullPath.Replace("\\", "/");
            return fullPath;
        }
    }
}
