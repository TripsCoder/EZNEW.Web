using EZNEW.Framework.Extension;
using EZNEW.Framework.IoC;
using EZNEW.Framework.Net;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using EZNEW.Framework.Serialize;
using EZNEW.Framework.Net.Http;
using EZNEW.Framework.Upload;
using EZNEW.Web.Utility;
using Microsoft.AspNetCore.Http;
using EZNEW.Framework.Upload.Config;

namespace EZNEW.Web.Upload
{
    /// <summary>
    /// Web上传工具
    /// </summary>
    public static class WebUploadManager
    {
        static WebUploadManager()
        {
            var uploadConfig = ContainerManager.Resolve<IOptions<UploadConfig>>()?.Value;
            UploadManager.ConfigUpload(uploadConfig);
        }

        const string DEFAULT_CONTENT_ROOT = "wwwroot";

        #region upload by http request

        /// <summary>
        /// upload by request
        /// </summary>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByRequestAsync()
        {
            var request = HttpContextHelper.Current.Request;
            return await UploadByRequestAsync(request).ConfigureAwait(false);
        }

        /// <summary>
        /// upload by request
        /// </summary>
        /// <param name="request">http request</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByRequestAsync(HttpRequest request)
        {
            if (request == null)
            {
                return UploadResult.Empty;
            }
            var uploadParameter = JsonSerialize.JsonToObject<RemoteUploadParameter>(request.Form[RemoteUploadParameter.REQUEST_KEY]);
            if (uploadParameter == null)
            {
                return UploadResult.Empty;
            }
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();
            if (!request.Form.Files.IsNullOrEmpty())
            {
                foreach (var file in request.Form.Files)
                {
                    files.Add(file.FileName, file.OpenReadStream().ToBytes());
                }
            }
            return await UploadByConfigAsync(uploadParameter.Files, files, HttpRequestHelper.GetHttpContextParameters(request)).ConfigureAwait(false);
        }

        #endregion

        #region upload by config

        /// <summary>
        /// upload by config
        /// </summary>
        /// <param name="fileOption">file option</param>
        /// <param name="file">upload file</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByConfigAsync(UploadFile fileOption, byte[] file, object parameters = null)
        {
            if (file == null || file.Length <= 0)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (fileOption == null)
            {
                throw new ArgumentNullException(nameof(fileOption));
            }
            Dictionary<string, string> parametersDic = null;
            if (parameters != null)
            {
                parametersDic = parameters.ObjectToStringDcitionary();
            }
            return await UploadByConfigAsync(fileOption, file, parametersDic).ConfigureAwait(false);
        }

        /// <summary>
        /// upload by config
        /// </summary>
        /// <param name="fileOption">file option</param>
        /// <param name="file">upload file</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByConfigAsync(UploadFile fileOption, byte[] file, Dictionary<string, string> parameters = null)
        {
            if (file == null || file.Length <= 0)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (fileOption == null)
            {
                throw new ArgumentNullException(nameof(fileOption));
            }
            Dictionary<string, byte[]> files = new Dictionary<string, byte[]>()
            {
                {
                    fileOption.FileName,
                    file
                }
            };
            return await UploadByConfigAsync(new List<UploadFile>() { fileOption }, files, parameters);
        }

        /// <summary>
        /// upload by config
        /// </summary>
        /// <param name="fileOptions">file options</param>
        /// <param name="files">upload files</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByConfigAsync(IEnumerable<UploadFile> fileOptions, Dictionary<string, byte[]> files, object parameters = null)
        {
            Dictionary<string, string> parameterDic = null;
            if (parameters != null)
            {
                parameterDic = parameters.ObjectToStringDcitionary();
            }
            return await UploadByConfigAsync(fileOptions, files, parameterDic).ConfigureAwait(false);
        }

        /// <summary>
        /// upload by config
        /// </summary>
        /// <param name="fileOptions">file options</param>
        /// <param name="files">upload files</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        public static async Task<UploadResult> UploadByConfigAsync(IEnumerable<UploadFile> fileOptions, Dictionary<string, byte[]> files, Dictionary<string, string> parameters = null)
        {
            var result = await UploadManager.UploadByConfigAsync(fileOptions, files, parameters).ConfigureAwait(false);
            return HandleUploadResult(result);
        }

        #endregion

        #region remote upload

        /// <summary>
        /// remote upload file
        /// </summary>
        /// <param name="remoteOption">remote option</param>
        /// <param name="fileOptions">file options</param>
        /// <param name="files">upload files</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        public static async Task<UploadResult> RemoteUploadAsync(RemoteOption remoteOption, List<UploadFile> fileOptions, Dictionary<string, byte[]> files, Dictionary<string, string> parameters = null)
        {
            var result = await UploadManager.RemoteUploadAsync(remoteOption, fileOptions, files, parameters).ConfigureAwait(false);
            return HandleUploadResult(result);
        }

        /// <summary>
        /// remote upload file
        /// </summary>
        /// <param name="remoteOption">remote options</param>
        /// <param name="fileOption">file option</param>
        /// <param name="file">upload file</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        public static async Task<UploadResult> RemoteUploadAsync(RemoteOption remoteOption, UploadFile fileOption, byte[] file, object parameters = null)
        {
            return await RemoteUploadAsync(remoteOption, fileOption, file, parameters).ConfigureAwait(false);
        }

        #endregion

        #region local upload

        /// <summary>
        /// local upload file
        /// </summary>
        /// <param name="uploadOption">upload option</param>
        /// <param name="fileOptions">file options</param>
        /// <param name="files">files</param>
        /// <returns></returns>
        public static async Task<UploadResult> LocalUploadAsync(UploadOption uploadOption, List<UploadFile> fileOptions, Dictionary<string, byte[]> files)
        {
            var result = await UploadManager.LocalUploadAsync(uploadOption, fileOptions, files).ConfigureAwait(false);
            return HandleUploadResult(result);
        }

        /// <summary>
        ///  local upload file
        /// </summary>
        /// <param name="uploadOption">upload option</param>
        /// <param name="fileOption">file option</param>
        /// <param name="file">file</param>
        /// <returns></returns>
        public static async Task<UploadResult> LocalUploadAsync(UploadOption uploadOption, UploadFile fileOption, byte[] file)
        {
            return await LocalUploadAsync(uploadOption, fileOption, file).ConfigureAwait(false);
        }

        #endregion

        #region handle upload result

        /// <summary>
        /// hanndle upload result
        /// </summary>
        /// <param name="originResult">origin result</param>
        /// <returns></returns>
        static UploadResult HandleUploadResult(UploadResult originResult)
        {
            if (originResult == null)
            {
                return null;
            }
            originResult.Files?.ForEach(r =>
            {
                r.FullPath = Client.GetFullPath(r.RelativePath.LSplit(DEFAULT_CONTENT_ROOT)[0]);
            });
            return originResult;
        }

        #endregion
    }
}
