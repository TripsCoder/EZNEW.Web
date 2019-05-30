using EZNEW.Framework.Extension;
using EZNEW.Framework.IoC;
using EZNEW.Framework.Net;
using EZNEW.Web.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using EZNEW.Framework.Serialize;
using EZNEW.Web.Config.Upload;
using EZNEW.Framework.Net.Upload;
using EZNEW.Framework.Net.Http;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// Web上传工具
    /// </summary>
    public static class WebUploadHelper
    {
        /// <summary>
        /// 根据上传配置上传文件
        /// </summary>
        /// <param name="fileContent">上传文件</param>
        /// <param name="parameters">参数信息</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByConfigAsync(UploadFile fileConfig, byte[] fileContent, object parameters = null)
        {
            if (fileContent == null || fileContent.Length <= 0)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }
            if (fileConfig == null)
            {
                throw new ArgumentNullException(nameof(fileConfig));
            }
            Dictionary<string, string> parametersDic = null;
            if (parameters != null)
            {
                parametersDic = parameters.ObjectToStringDcitionary();
            }
            return await UploadByConfigAsync(fileConfig, fileContent, parametersDic).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据上传配置上传文件
        /// </summary>
        /// <param name="uploadConfigKey">上传配置键值</param>
        /// <param name="fileContent">上传文件</param>
        /// <param name="parameters">参数信息</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByConfigAsync(UploadFile fileConfig, byte[] fileContent, Dictionary<string, string> parameters = null)
        {
            if (fileContent == null || fileContent.Length <= 0)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }
            if (fileConfig == null)
            {
                throw new ArgumentNullException(nameof(fileConfig));
            }
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>()
            {
                {
                    fileConfig.FileName,
                    fileContent
                }
            };
            return await UploadByConfigAsync(new List<UploadFile>() { fileConfig }, files, parameters);
        }

        /// <summary>
        /// 根据上传配置上传文件
        /// </summary>
        /// <param name="uploadConfigKey">上传配置键值</param>
        /// <param name="fileContent">上传文件</param>
        /// <param name="parameters">参数信息</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByConfigAsync(IEnumerable<UploadFile> fileConfigs, Dictionary<string, byte[]> fileContents, object parameters = null)
        {
            Dictionary<string, string> parameterDic = null;
            if (parameters != null)
            {
                parameterDic = parameters.ObjectToStringDcitionary();
            }
            return await UploadByConfigAsync(fileConfigs, fileContents, parameterDic).ConfigureAwait(false);
        }

        /// <summary>
        /// 根据上传配置上传文件
        /// </summary>
        /// <param name="uploadConfigKey">上传配置键值</param>
        /// <param name="fileContent">上传文件</param>
        /// <param name="parameters">参数信息</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByConfigAsync(IEnumerable<UploadFile> fileConfigs, Dictionary<string, byte[]> fileContents, Dictionary<string, string> parameters = null)
        {
            if (fileContents == null || fileContents.Count <= 0)
            {
                throw new ArgumentNullException(nameof(fileContents));
            }
            var uploadConfig = ContainerManager.Resolve<IOptions<UploadConfig>>()?.Value;
            string defaultPath = "upload";
            if (uploadConfig == null)
            {
                uploadConfig = new UploadConfig()
                {
                    Default = new UploadConfigOption()
                    {
                        Remote = false,
                        SavePath = defaultPath
                    }
                };
            }
            List<string> fileTypeGroups = fileConfigs.Select(c => c.FileType).Distinct().ToList();
            UploadResult uploadResult = null;
            foreach (var fileType in fileTypeGroups)
            {
                var uploadOption = uploadConfig.GetOption(fileType);
                if (uploadOption == null)
                {
                    uploadOption = new UploadConfigOption()
                    {
                        Remote = false,
                        SavePath = defaultPath
                    };
                }
                var groupFileConfigs = fileConfigs.Where(c => c.FileType == fileType).ToList();
                var groupFiles = fileContents.Where(c => groupFileConfigs.Exists(fc => fc.FileName == c.Key)).ToDictionary(c => c.Key, c => c.Value);
                UploadResult groupResult = null;
                if (uploadOption.Remote)
                {
                    groupResult = await RemoteUploadAsync(uploadOption.GetRandomRemoteUploadServer(), groupFileConfigs, groupFiles, parameters).ConfigureAwait(false);
                }
                else
                {
                    groupResult = await LocalUploadAsync(uploadOption, groupFileConfigs, groupFiles).ConfigureAwait(false);
                }
                if (groupResult == null)
                {
                    continue;
                }
                if (uploadResult == null)
                {
                    uploadResult = groupResult;
                }
                else
                {
                    uploadResult.Combine(groupResult);
                }
            }
            return uploadResult;
        }

        /// <summary>
        /// 执行远程上传
        /// </summary>
        /// <param name="remoteOption">远程上传配置</param>
        /// <param name="files">上传文件</param>
        /// <param name="parameters">上传参数</param>
        /// <returns></returns>
        static async Task<UploadResult> RemoteUploadAsync(RemoteUploadOption remoteOption, List<UploadFile> fileConfigs, Dictionary<string, byte[]> files, Dictionary<string, string> parameters = null)
        {
            if (remoteOption == null || string.IsNullOrWhiteSpace(remoteOption.Server))
            {
                throw new ArgumentNullException(nameof(remoteOption));
            }
            if (files == null || files.Count <= 0)
            {
                throw new ArgumentNullException(nameof(files));
            }
            UploadOption uploadOption = new UploadOption()
            {
                Files = fileConfigs
            };
            parameters = parameters ?? new Dictionary<string, string>();
            parameters.Add(UploadOption.REQUEST_KEY, JsonSerialize.ObjectToJson(uploadOption));
            string url = remoteOption.GetUploadUrl();
            return await HttpUtil.UploadAsync(url, files, parameters).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行远程上传
        /// </summary>
        /// <param name="remoteOption">远程上传配置</param>
        /// <param name="fileContent">上传文件</param>
        /// <param name="parameters">上传参数</param>
        /// <returns></returns>
        static async Task<UploadResult> RemoteUploadAsync(RemoteUploadOption remoteOption, UploadFile fileConfig, byte[] fileContent, object parameters = null)
        {
            if (fileConfig == null)
            {
                throw new ArgumentNullException(nameof(fileConfig));
            }
            Dictionary<string, string> parameterDic = null;
            if (parameters != null)
            {
                parameterDic = parameters.ObjectToStringDcitionary();
            }
            return await RemoteUploadAsync(remoteOption, new List<UploadFile>() { fileConfig }, new Dictionary<string, byte[]>() { { "file1", fileContent } }, parameterDic).ConfigureAwait(false);
        }

        /// <summary>
        /// 本地上传文件
        /// </summary>
        /// <param name="uploadConfig">上传配置信息</param>
        /// <param name="fileConfigs">文件配置</param>
        /// <param name="files">文件信息</param>
        /// <returns></returns>
        static async Task<UploadResult> LocalUploadAsync(UploadConfigOption uploadConfig, List<UploadFile> fileConfigs, Dictionary<string, byte[]> files)
        {
            if (fileConfigs.IsNullOrEmpty() || files == null || files.Count <= 0)
            {
                return new UploadResult()
                {
                    Code = "400",
                    ErrorMsg = "没有指定任何上传文件配置或文件信息",
                    Success = false
                };
            }
            UploadResult result = new UploadResult()
            {
                Success = true,
                FileInfoList = new List<UploadFileResult>()
            };
            foreach (var fileItem in files)
            {
                string fileName = fileItem.Key;
                var fileConfig = fileConfigs.FirstOrDefault(c => c.FileName == fileName);
                if (fileConfig == null)
                {
                    fileConfig = new UploadFile()
                    {
                        FileName = fileName,
                        Rename = false
                    };
                }
                var fileResult = await LocalUploadFileAsync(uploadConfig, fileConfig, fileItem.Value);
                result.FileInfoList.Add(fileResult);
            }
            return result;
        }

        /// <summary>
        ///  本地上传
        /// </summary>
        /// <param name="uploadConfig">上传配置</param>
        /// <param name="fileConfig">文件配置</param>
        /// <param name="file">上传文件</param>
        /// <returns></returns>
        static async Task<UploadResult> LocalUploadAsync(UploadConfigOption uploadConfig, UploadFile fileConfig, byte[] file)
        {
            var fileResult = await LocalUploadFileAsync(uploadConfig, fileConfig, file).ConfigureAwait(false);
            return new UploadResult()
            {
                Code = "200",
                Success = true,
                FileInfoList = new List<UploadFileResult>()
                {
                    fileResult
                }
            };
        }

        /// <summary>
        /// 本地上传文件
        /// </summary>
        /// <param name="uploadConfig">上传配置</param>
        /// <param name="fileConfig">文件配置</param>
        /// <param name="file">上传文件</param>
        /// <returns></returns>
        static async Task<UploadFileResult> LocalUploadFileAsync(UploadConfigOption uploadConfig, UploadFile fileConfig, byte[] file)
        {
            if (uploadConfig == null)
            {
                throw new ArgumentNullException(nameof(uploadConfig));
            }
            if (fileConfig == null)
            {
                throw new ArgumentNullException(nameof(fileConfig));
            }
            string savePath = Path.Combine(uploadConfig.SavePath, fileConfig.Folder ?? string.Empty);
            string suffix = Path.GetExtension(fileConfig.FileName).Trim('.');
            if (!string.IsNullOrWhiteSpace(fileConfig.Suffix))
            {
                suffix = fileConfig.Suffix.Trim('.');
            }
            string fileName = Path.GetFileNameWithoutExtension(fileConfig.FileName);
            if (fileConfig.Rename)
            {
                fileName = Guid.NewGuid().ToInt64().ToString();
            }
            fileName = string.Format("{0}.{1}", fileName, suffix);
            var fileResult = await SaveUploadFileAsync(savePath, uploadConfig.SaveToContentRoot, uploadConfig.ContentRootPath, fileName, file).ConfigureAwait(false);
            fileResult.OriginalFileName = fileConfig.FileName;
            return fileResult;
        }

        /// <summary>
        /// 保存上传文件
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="file">文件内容</param>
        /// <returns></returns>
        static async Task<UploadFileResult> SaveUploadFileAsync(string savePath, bool saveToContentRoot, string contentRootFolder, string fileName, byte[] file)
        {
            if (file == null || file.Length <= 0)
            {
                throw new ArgumentNullException(nameof(file));
            }
            string realSavePath = savePath;
            if (string.IsNullOrWhiteSpace(realSavePath))
            {
                realSavePath = Directory.GetCurrentDirectory();
            }
            if (!Path.IsPathRooted(realSavePath))
            {
                if (saveToContentRoot)
                {
                    realSavePath = Path.Combine(string.IsNullOrWhiteSpace(contentRootFolder) ? "wwwroot" : contentRootFolder, realSavePath);
                }
                realSavePath = Path.Combine(Directory.GetCurrentDirectory(), realSavePath);
            }
            if (!Directory.Exists(realSavePath))
            {
                Directory.CreateDirectory(realSavePath);
            }
            string fullFilePath = Path.Combine(realSavePath, fileName);
            File.WriteAllBytes(fullFilePath, file);
            string relativePath = Path.Combine(savePath, fileName);
            var result = new UploadFileResult()
            {
                FileName = fileName,
                FullPath = Client.GetFullPath(relativePath),
                Suffix = Path.GetExtension(fileName).Trim('.'),
                RelativePath = relativePath,
                UploadDate = DateTime.Now
            };
            return await Task.FromResult(result);
        }
    }
}
