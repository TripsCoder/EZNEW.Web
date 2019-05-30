using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Security.Authentication.Cookie.Ticket
{
    /// <summary>
    /// 授权凭据验证
    /// </summary>
    public interface IAuthenticationTicketStore: ITicketStore
    {
        /// <summary>
        /// 验证认证凭据
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="sessionToken">session token</param>
        /// <param name="renew">刷新凭据</param>
        /// <returns></returns>
        Task<bool> VerifyTicketAsync(string subject,string sessionToken, bool renew = true);

        /// <summary>
        /// 获取授权对象对应的session key
        /// </summary>
        /// <param name="subject">授权对象</param>
        /// <returns></returns>
        Task<string> GetSessionTokenAsync(string subject);
    }
}
