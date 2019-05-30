using EZNEW.Framework.Extension;
using EZNEW.Web.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using EZNEW.Framework.IoC;
using Microsoft.Extensions.Options;
using EZNEW.Web.Security.Authentication.Cookie;
using Microsoft.AspNetCore.Authentication.Cookies;
using EZNEW.Web.Security.Authentication.Cookie.Ticket;
using System.Security.Claims;

namespace EZNEW.Web.Security.Authentication.Session
{

    /// <summary>
    /// Session管理
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// 验证指定的session是否有效
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="sessionToken">session token</param>
        /// <returns></returns>
        public static async Task<bool> VerifySessionAsync(string subject, string sessionToken)
        {
            if (sessionToken.IsNullOrEmpty())
            {
                return false;
            }
            CookieAuthenticationOptions cookieOptions = ContainerManager.Resolve<IOptionsMonitor<CookieAuthenticationOptions>>().Get(CookieAuthenticationDefaults.AuthenticationScheme);
            if (cookieOptions?.SessionStore != null)
            {
                var sessionStore = cookieOptions.SessionStore as IAuthenticationTicketStore;
                if (sessionStore != null)
                {
                    return await sessionStore.VerifyTicketAsync(subject, sessionToken).ConfigureAwait(false);
                }
            }
            return true;
        }

        /// <summary>
        /// 验证当前请求的session是否有效
        /// </summary>
        /// <param name="claims">验证凭据信息</param>
        /// <returns></returns>
        public static async Task<bool> VerifySessionAsync(IEnumerable<Claim> claims)
        {
            if (claims == null || !claims.Any())
            {
                return false;
            }
            var sessionToken = AuthSession.GetSessionToken(claims);
            var subject = AuthSession.GetSubject(claims);
            return await VerifySessionAsync(subject, sessionToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取session token
        /// </summary>
        /// <param name="subject">授权对象</param>
        /// <returns></returns>
        public static async Task<string> GetSessionToken(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                return string.Empty;
            }
            CookieAuthenticationOptions cookieOptions = ContainerManager.Resolve<IOptionsMonitor<CookieAuthenticationOptions>>().Get(CookieAuthenticationDefaults.AuthenticationScheme);
            if (cookieOptions?.SessionStore != null)
            {
                var sessionStore = cookieOptions.SessionStore as IAuthenticationTicketStore;
                if (sessionStore != null)
                {
                    return await sessionStore.GetSessionTokenAsync(subject).ConfigureAwait(false);
                }
            }
            return string.Empty;
        }
    }
}
