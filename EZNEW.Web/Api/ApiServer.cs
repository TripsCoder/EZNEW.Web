using EZNEW.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Api
{
    /// <summary>
    /// api server
    /// </summary>
    public class ApiServer
    {
        /// <summary>
        /// server name
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// server host
        /// </summary>
        public string Host
        {
            get; set;
        }

        /// <summary>
        /// route path
        /// </summary>
        public string RoutePath
        {
            get; set;
        } = "api";

        /// <summary>
        /// get endpoint path
        /// </summary>
        /// <param name="endpointPath">节点相对路径</param>
        /// <returns></returns>
        public string GetEndpointPath(string endpointPath)
        {
            if (Host.IsNullOrEmpty())
            {
                return string.Empty;
            }
            string serverPath = Host.Trim('/');
            if (!string.IsNullOrEmpty(RoutePath))
            {
                serverPath += "/" + RoutePath.Trim('/');
            }
            if (string.IsNullOrWhiteSpace(endpointPath))
            {
                return serverPath;
            }
            return string.Format("{0}/{1}", serverPath, endpointPath.Trim('/'));
        }

        /// <summary>
        /// get endpoint path
        /// </summary>
        /// <param name="endpoint">api endpoint</param>
        /// <returns></returns>
        public string GetEndpointPath(ApiEndpoint endpoint)
        {
            if (endpoint == null)
            {
                return string.Empty;
            }
            return GetEndpointPath(endpoint.Path);
        }
    }
}
