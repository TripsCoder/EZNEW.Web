using System;
using System.Collections.Generic;
using System.Text;
using EZNEW.Framework.Extension;

namespace EZNEW.Web.Api
{
    /// <summary>
    /// api group
    /// </summary>
    public class ApiObject
    {
        /// <summary>
        /// api group name
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// service choice pattern
        /// </summary>
        public ServerChoicePattern ServerChoicePattern
        {
            get; set;
        } = ServerChoicePattern.Random;

        /// <summary>
        /// api servers
        /// </summary>
        public List<ApiServer> Servers
        {
            get; set;
        }

        /// <summary>
        /// api endpoints
        /// </summary>
        public List<ApiEndpoint> Endpoints
        {
            get; set;
        }

        /// <summary>
        /// get server
        /// </summary>
        /// <returns></returns>
        public ApiServer GetServer()
        {
            if (Servers.IsNullOrEmpty())
            {
                return null;
            }
            if (Servers.Count == 1)
            {
                return Servers[0];
            }
            ApiServer apiServer = null;
            switch (ServerChoicePattern)
            {
                case ServerChoicePattern.First:
                    apiServer = Servers[0];
                    break;
                case ServerChoicePattern.Latest:
                    apiServer = Servers[Servers.Count - 1];
                    break;
                default:
                    var random = new Random();
                    int ranIndex = random.Next(0, Servers.Count);
                    apiServer = Servers[ranIndex];
                    break;
            }
            return apiServer;
        }
    }
}
