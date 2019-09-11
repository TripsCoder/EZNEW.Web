using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EZNEW.Framework.Extension;
using Microsoft.AspNetCore.Http;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// url helper
    /// </summary>
    public static class UrlHelper
    {
        #region Url Encode/Decode

        /// <summary>
        /// url encode
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static string UrlEncode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            return HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// url decode
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static string UrlDecode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            return HttpUtility.UrlDecode(url);
        }

        #endregion

        #region url parameter

        /// <summary>
        /// get url without parameter
        /// </summary>
        /// <param name="url">url</param>
        /// <returns></returns>
        public static string GetUrlWithOutParameter(string url)
        {
            if (url.IsNullOrEmpty())
            {
                return url;
            }
            string[] urlArray = url.LSplit("?");
            if (urlArray.Length <= 0)
            {
                return string.Empty;
            }
            return urlArray[0];
        }

        /// <summary>
        /// remove parameters
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="parameters">parameters</param>
        /// <param name="removeParameterNames">parameter names</param>
        /// <returns></returns>
        public static string RemoveUrlParameters(string url, IDictionary<string, string> parameters, IEnumerable<string> removeParameterNames)
        {
            if (removeParameterNames.IsNullOrEmpty() || url.IsNullOrEmpty() || parameters == null || parameters.Count <= 0)
            {
                return url;
            }
            List<string> removeNames = removeParameterNames.Select(c => c.ToLower()).ToList();
            url = GetUrlWithOutParameter(url).Trim('/', '?', '&');
            List<string> parameterValues = new List<string>(parameters.Count);
            foreach (var parameterItem in parameters)
            {
                string parameterName = parameterItem.Key.ToLower();
                if (removeNames.Contains(parameterName))
                {
                    continue;
                }
                parameterValues.Add(string.Format("{0}={1}", parameterName, UrlEncode(parameterItem.Value)));
            }
            if (parameterValues.IsNullOrEmpty())
            {
                return url;
            }
            return string.Format("{0}?{1}", url, string.Join("&", parameterValues));
        }

        /// <summary>
        /// remove url parameters
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="removeParameterNames">remove parameter names</param>
        /// <returns></returns>
        public static string RemoveUrlParameters(HttpRequest request, IEnumerable<string> removeParameterNames)
        {
            if (request == null)
            {
                return string.Empty;
            }
            string[] queryParameterNames = request.Query.Keys.ToArray();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (string parameterKey in removeParameterNames)
            {
                if (parameterKey.IsNullOrEmpty())
                {
                    continue;
                }
                string parameterValue = request.Query[parameterKey];
                if (parameterValue.IsNullOrEmpty())
                {
                    continue;
                }
                parameters.Add(parameterKey, parameterValue);
            }
            return RemoveUrlParameters(request.Path, parameters, removeParameterNames);
        }

        /// <summary>
        /// append parameters
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="parameters">parameters</param>
        /// <returns></returns>
        public static string AppendParameters(string url, IDictionary<string, string> parameters)
        {
            if (url.IsNullOrEmpty() || parameters == null || parameters.Count <= 0)
            {
                return string.Empty;
            }
            Dictionary<string, string> nowParameters = new Dictionary<string, string>();
            string[] urlArray = url.LSplit("?");
            if (urlArray.Length > 1)
            {
                string urlParameterString = urlArray[1];
                var urlParameterValues = HttpUtility.ParseQueryString(urlParameterString);
                string[] parameterKeys = urlParameterValues.AllKeys;
                foreach (string key in parameterKeys)
                {
                    nowParameters.Add(key.ToLower(), urlParameterValues[key]);
                }
            }
            foreach (var newParameter in parameters)
            {
                string keyName = newParameter.Key.ToLower();
                if (nowParameters.ContainsKey(keyName))
                {
                    nowParameters[keyName] = newParameter.Value;
                }
                else
                {
                    nowParameters.Add(keyName, newParameter.Value);
                }
            }
            url = GetUrlWithOutParameter(url);
            if (nowParameters == null || nowParameters.Count <= 0)
            {
                return url;
            }
            List<string> parameterValueString = new List<string>(nowParameters.Count);
            foreach (var parameter in nowParameters)
            {
                parameterValueString.Add(string.Format("{0}={1}", parameter.Key, UrlEncode(parameter.Value)));
            }
            return string.Format("{0}?{1}", url, string.Join("&", parameterValueString));
        }

        #endregion
    }
}
