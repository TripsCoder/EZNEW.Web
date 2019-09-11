using EZNEW.Framework.Serialize;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// http response extensions
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// write json
        /// </summary>
        /// <param name="response">response</param>
        /// <param name="data">data</param>
        /// <returns></returns>
        public static async Task WriteJsonAsync(this HttpResponse response, object data)
        {
            var json = JsonSerialize.ObjectToJson(data);
            await response.WriteJsonAsync(json);
        }

        /// <summary>
        /// write json
        /// </summary>
        /// <param name="response">response</param>
        /// <param name="json">json data</param>
        /// <returns></returns>
        public static async Task WriteJsonAsync(this HttpResponse response, string json)
        {
            response.ContentType = "application/json; charset=UTF-8";
            await response.WriteAsync(json);
        }

        /// <summary>
        /// set cache
        /// </summary>
        /// <param name="response">response</param>
        /// <param name="maxAge">cache max age</param>
        public static void SetCache(this HttpResponse response, int maxAge)
        {
            if (maxAge == 0)
            {
                SetNoCache(response);
            }
            else if (maxAge > 0)
            {
                if (!response.Headers.ContainsKey("Cache-Control"))
                {
                    response.Headers.Add("Cache-Control", $"max-age={maxAge}");
                }
            }
        }

        /// <summary>
        /// set no cache
        /// </summary>
        /// <param name="response">http response</param>
        public static void SetNoCache(this HttpResponse response)
        {
            if (!response.Headers.ContainsKey("Cache-Control"))
            {
                response.Headers.Add("Cache-Control", "no-store, no-cache, max-age=0");
            }
            if (!response.Headers.ContainsKey("Pragma"))
            {
                response.Headers.Add("Pragma", "no-cache");
            }
        }

        /// <summary>
        /// write html
        /// </summary>
        /// <param name="response">responnse</param>
        /// <param name="html">html</param>
        /// <returns></returns>
        public static async Task WriteHtmlAsync(this HttpResponse response, string html)
        {
            response.ContentType = "text/html; charset=UTF-8";
            await response.WriteAsync(html, Encoding.UTF8);
        }
    }
}
