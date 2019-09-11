using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// http request extensions
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// wheather is ajax request
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return string.Equals(request.Query["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal) ||
                string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal);
        }

        /// <summary>
        /// get parameter value
        /// </summary>
        /// <param name="request">request</param>
        /// <param name="key">parameter key</param>
        /// <returns></returns>
        public static string GetValue(this HttpRequest request, string key)
        {
            if (string.IsNullOrWhiteSpace(key) || request == null)
            {
                return string.Empty;
            }
            try
            {
                if (request.Query.ContainsKey(key))
                {
                    return request.Query[key];
                }
                if (request.Form.ContainsKey(key))
                {
                    return request.Form[key];
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
