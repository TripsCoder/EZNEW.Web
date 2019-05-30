using System;
using System.Collections.Generic;
using System.Text;
using EZNEW.Framework.Extension;

namespace EZNEW.Web.Security.Authorization
{
    /// <summary>
    /// Authorize Config
    /// </summary>
    public class AuthorizeConfig
    {
        /// <summary>
        /// Servers
        /// </summary>
        public List<string> Servers
        {
            get; set;
        }

        /// <summary>
        /// Remote Authorize Verify
        /// </summary>
        public bool RemoteAuthorizeVerify
        {
            get; set;
        }

        /// <summary>
        /// Get Random AuthorizeServer
        /// </summary>
        /// <returns></returns>
        public string GetRandomServer()
        {
            if (Servers.IsNullOrEmpty())
            {
                return string.Empty;
            }
            if (Servers.Count == 1)
            {
                return Servers[0];
            }
            Random random = new Random();
            int ranIndex = random.Next(0, Servers.Count);
            return Servers[ranIndex];
        }
    }
}
