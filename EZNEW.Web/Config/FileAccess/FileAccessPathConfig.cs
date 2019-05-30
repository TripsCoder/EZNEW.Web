using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Config.FileAccess
{
    /// <summary>
    /// File Access Path Config
    /// </summary>
    public class FileAccessPathConfig
    {
        /// <summary>
        /// Default Option
        /// </summary>
        public FileAccessPathOption Default
        {
            get; set;
        }

        /// <summary>
        /// file access path options
        /// </summary>
        public Dictionary<string, FileAccessPathOption> Items
        {
            get; set;
        }

        /// <summary>
        /// get random file access path
        /// </summary>
        /// <param name="filePath">relative file path</param>
        /// <returns></returns>
        public string GetRandomFileFullPath(string key, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return string.Empty;
            }
            var fileAccessOption = Default;
            if (!string.IsNullOrWhiteSpace(key) && Items != null && Items.ContainsKey(key))
            {
                fileAccessOption = Items[key];
            }
            if (fileAccessOption == null)
            {
                return filePath;
            }
            return fileAccessOption.GetRandomFileFullPath(filePath);
        }
    }
}
