using EZNEW.Framework.Extension;
using EZNEW.Framework.IoC;
using EZNEW.Web.FileAccess.Config;
using EZNEW.Web.Utility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.FileAccess
{
    /// <summary>
    /// file access manager
    /// </summary>
    public static class FileAccessManager
    {
        static FileAccessManager()
        {
            var fileAccessConfig = ContainerManager.Resolve<IOptions<FileAccessConfig>>()?.Value;
            ConfigFileAccess(fileAccessConfig);
        }

        #region propertys

        /// <summary>
        /// default file option
        /// </summary>
        static FileAccessOption Default = null;

        /// <summary>
        /// file options
        /// key:file object name
        /// value:file object access option
        /// </summary>
        static Dictionary<string, FileAccessOption> FileOptions = new Dictionary<string, FileAccessOption>();

        #endregion

        #region method

        #region connfig file access

        /// <summary>
        /// config file access
        /// </summary>
        /// <param name="fileAccessConfig">file access config</param>
        public static void ConfigFileAccess(FileAccessConfig fileAccessConfig)
        {
            if (fileAccessConfig == null)
            {
                return;
            }
            ConfigDefaultFileAccess(fileAccessConfig.Default);
            ConfigFileObjectAccess(fileAccessConfig.FileObjects?.ToArray());
        }

        #endregion

        #region config default file access option

        /// <summary>
        /// config default file access option
        /// </summary>
        /// <param name="fileAccessOption">file access option</param>
        public static void ConfigDefaultFileAccess(FileAccessOption fileAccessOption)
        {
            Default = fileAccessOption;
        }

        #endregion

        #region config file object access

        /// <summary>
        /// config file object
        /// </summary>
        /// <param name="fileObjects">file objects</param>
        public static void ConfigFileObjectAccess(params FileObject[] fileObjects)
        {
            if (fileObjects.IsNullOrEmpty())
            {
                return;
            }
            foreach (var fileObject in fileObjects)
            {
                if (fileObject == null || fileObject.Name.IsNullOrEmpty() || fileObject.FileAccessOption == null)
                {
                    continue;
                }
                FileOptions[fileObject.Name] = fileObject.FileAccessOption;
            }
        }

        #endregion

        #region get local full path

        /// <summary>
        /// get local full path
        /// </summary>
        /// <param name="fileRelativePath">file relative path</param>
        /// <returns></returns>
        public static string GetLocalFullPath(string fileRelativePath)
        {
            return Client.GetFullPath(fileRelativePath);
        }

        #endregion

        #region get file full path

        /// <summary>
        /// get file full path
        /// </summary>
        /// <param name="fileObjectName">file object name</param>
        /// <param name="fileRelativePath">file relative path</param>
        /// <returns></returns>
        public static string GetFileFullPath(string fileObjectName, string fileRelativePath)
        {
            FileOptions.TryGetValue(fileObjectName, out var fileAccessOption);
            if (fileAccessOption == null)
            {
                fileAccessOption = Default;
            }
            return GetFileFullPath(fileAccessOption, fileRelativePath);
        }

        /// <summary>
        /// get file full path
        /// </summary>
        /// <param name="fileAccessOption">file access option</param>
        /// <param name="fileRelativePath">file path</param>
        /// <returns></returns>
        public static string GetFileFullPath(FileAccessOption fileAccessOption, string fileRelativePath)
        {
            return fileAccessOption?.GetFilePath(fileRelativePath) ?? fileRelativePath;
        }

        #endregion

        #endregion
    }
}
