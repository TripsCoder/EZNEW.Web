using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// http request helper
    /// </summary>
    public static class HttpRequestHelper
    {
        /// <summary>
        /// get all request parameters
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetHttpContextParameters(HttpRequest request)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            IEnumerable<KeyValuePair<string, StringValues>> collection;
            if (request.Method.ToUpper() == "POST")
            {
                collection = request.Form;
            }
            else
            {
                collection = request.Query;
            }

            foreach (var item in collection)
            {
                if (parameters.ContainsKey(item.Key))
                {
                    parameters[item.Key] = item.Value;
                }
                else
                {
                    parameters.Add(item.Key, item.Value);
                }
            }
            return parameters;
        }

        /// <summary>
        /// get all request parameters
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        public static SortedDictionary<string, string> GetHttpContextSortParameters(HttpRequest request)
        {
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>();
            IEnumerable<KeyValuePair<string, StringValues>> collection;
            if (request.Method.ToUpper() == "POST")
            {
                collection = request.Form;
            }
            else
            {
                collection = request.Query;
            }

            foreach (var item in collection)
            {
                if (parameters.ContainsKey(item.Key))
                {
                    parameters[item.Key] = item.Value;
                }
                else
                {
                    parameters.Add(item.Key, item.Value);
                }
            }
            return parameters;
        }
    }
}
