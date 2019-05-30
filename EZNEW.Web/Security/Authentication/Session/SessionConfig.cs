using EZNEW.Framework.IoC;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Security.Authentication.Session
{
    /// <summary>
    /// session配置
    /// </summary>
    public class SessionConfig
    {
        /// <summary>
        /// 过期时间(默认2个小时)
        /// </summary>
        public TimeSpan Expires
        {
            get; set;
        } = TimeSpan.FromHours(2);

        /// <summary>
        /// Session Claim Name
        /// </summary>
        public string SessionClaimName
        {
            get;set;
        }= "micbeach_auth_session_name";

        /// <summary>
        /// 获取session配置
        /// </summary>
        /// <returns></returns>
        public static SessionConfig GetSessionConfig()
        {
            SessionConfig sessionConfig = null;
            if (ContainerManager.IsRegister<IOptions<SessionConfig>>())
            {
                sessionConfig = ContainerManager.Resolve<IOptions<SessionConfig>>()?.Value;
            }
            if (sessionConfig == null)
            {
                sessionConfig = new SessionConfig();
            }
            return sessionConfig;
        }
    }
}
