using EZNEW.Framework.Extension;
using EZNEW.Framework.IoC;
using EZNEW.Framework.Net;
using EZNEW.Framework.Net.Http;
using EZNEW.Framework.Serialize;
using EZNEW.Web.Api.Config;
using EZNEW.Web.Api.Request;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Api
{
    /// <summary>
    /// api manager
    /// </summary>
    public class ApiManager
    {
        static ApiManager()
        {
            var apiConfig = ContainerManager.Resolve<IOptions<ApiConfig>>()?.Value;
            InitApiConfig(apiConfig);
        }

        #region propertys

        /// <summary>
        /// api objects
        /// key: api object name
        /// value: api object
        /// </summary>
        static Dictionary<string, ApiObject> ApiObjects = new Dictionary<string, ApiObject>();

        /// <summary>
        /// endponit collection
        /// </summary>
        static Dictionary<string, ApiEndpoint> EndpointCollection = new Dictionary<string, ApiEndpoint>();

        #endregion

        #region methods

        #region init api config

        /// <summary>
        /// init api config
        /// </summary>
        static void InitApiConfig(ApiConfig apiConfig)
        {
            if (apiConfig == null)
            {
                return;
            }
            if (apiConfig.Apis?.IsNullOrEmpty() ?? true)
            {
                return;
            }
            foreach (var apiObject in apiConfig.Apis)
            {
                RegisterApiObject(apiObject);
            }
        }

        #endregion

        #region config api

        /// <summary>
        /// config api
        /// </summary>
        /// <param name="apiConfig">api config</param>
        public static void ConfigApi(ApiConfig apiConfig)
        {
            if (apiConfig == null)
            {
                return;
            }
            InitApiConfig(apiConfig);
        }

        #endregion

        #region format object&endpoint key

        /// <summary>
        /// format object&endpoint key
        /// </summary>
        /// <param name="apiObjectName">object name</param>
        /// <param name="endpointName">endpoint name</param>
        /// <returns></returns>
        static string GetApiObjectAndEndpointFormatKey(string apiObjectName, string endpointName)
        {
            return string.Format("{0}_{1}", apiObjectName, endpointName);
        }

        #endregion

        #region register api object

        /// <summary>
        /// register api object
        /// </summary>
        /// <param name="apiObject">api object</param>
        public static void RegisterApiObject(ApiObject apiObject)
        {
            if (apiObject == null || apiObject.Name.IsNullOrEmpty() || apiObject.Servers.IsNullOrEmpty() || apiObject.Endpoints.IsNullOrEmpty())
            {
                return;
            }
            ApiObjects[apiObject.Name] = apiObject;
            foreach (var endpoint in apiObject.Endpoints)
            {
                string formatKey = GetApiObjectAndEndpointFormatKey(apiObject.Name, endpoint.Name);
                EndpointCollection[formatKey] = endpoint;
            }
        }

        #endregion

        #region register api server

        /// <summary>
        /// register api server
        /// </summary>
        /// <param name="apiObjectName">api object name</param>
        /// <param name="server">api server</param>
        public static void RegisterServer(string apiObjectName, ApiServer server)
        {
            if (apiObjectName.IsNullOrEmpty() || server == null)
            {
                return;
            }
            if (!ApiObjects.TryGetValue(apiObjectName, out var nowApiGroup) || nowApiGroup == null)
            {
                ApiObjects[apiObjectName] = new ApiObject()
                {
                    Name = apiObjectName,
                    Endpoints = new List<ApiEndpoint>(),
                    Servers = new List<ApiServer>() { server }
                };
            }
            else
            {
                nowApiGroup.Servers.Add(server);
            }
        }

        #endregion

        #region register api endpoint

        /// <summary>
        /// register api endpoint
        /// </summary>
        /// <param name="apiObjectName">api object name</param>
        /// <param name="endpoint">api endpoint</param>
        public static void RegisterEndpoint(string apiObjectName, ApiEndpoint endpoint)
        {
            if (apiObjectName.IsNullOrEmpty() || endpoint == null)
            {
                return;
            }
            if (!ApiObjects.TryGetValue(apiObjectName, out var nowApiGroup) || nowApiGroup == null)
            {
                ApiObjects[apiObjectName] = new ApiObject()
                {
                    Name = apiObjectName,
                    Endpoints = new List<ApiEndpoint>() { endpoint },
                    Servers = new List<ApiServer>()
                };
            }
            else
            {
                nowApiGroup.Endpoints.Add(endpoint);
            }
            string formatKey = GetApiObjectAndEndpointFormatKey(apiObjectName, endpoint.Name);
            EndpointCollection[formatKey] = endpoint;
        }

        #endregion

        #region get api object

        /// <summary>
        /// get api object
        /// </summary>
        /// <param name="apiObjectName">api config name</param>
        /// <returns></returns>
        public static ApiObject GetApiObject(string apiObjectName)
        {
            if (string.IsNullOrWhiteSpace(apiObjectName))
            {
                return null;
            }
            ApiObjects.TryGetValue(apiObjectName, out var apiObject);
            return apiObject;
        }

        #endregion

        #region get api endpoint

        /// <summary>
        /// get api endpoint
        /// </summary>
        /// <param name="apiObjectName">api object name</param>
        /// <param name="endpointName">endpoint name</param>
        /// <returns></returns>
        public static ApiEndpoint GetApiEndpoint(string apiObjectName, string endpointName)
        {
            string formatKey = GetApiObjectAndEndpointFormatKey(apiObjectName, endpointName);
            EndpointCollection.TryGetValue(formatKey, out var endpoint);
            return endpoint;
        }

        #endregion

        #region execute api

        /// <summary>
        /// execute api
        /// </summary>
        /// <typeparam name="T">return data type</typeparam>
        /// <param name="apiObjectName">api object name </param>
        /// <param name="endpointName">endpoint</param>
        /// <param name="request">request</param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(string apiObjectName, string endpointName, ApiRequest request)
        {
            var apiObject = GetApiObject(apiObjectName);
            if (apiObject == null)
            {
                throw new Exception(string.Format("didn't config api {0}", apiObjectName));
            }
            var apiServer = apiObject.GetServer();
            if (apiServer == null)
            {
                throw new Exception(string.Format("set up at least one server info for api {0}", apiObjectName));
            }
            var endpoint = GetApiEndpoint(apiObjectName, endpointName);
            if (endpoint == null)
            {
                throw new Exception(string.Format("didn't set endpoint for {0}", endpointName));
            }
            var response = await HttpUtil.HttpPostJsonAsync(apiServer.GetEndpointPath(endpoint.Path), request).ConfigureAwait(false);
            string stringValue = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerialize.JsonToObject<T>(stringValue);
        }

        #endregion

        #endregion
    }
}
