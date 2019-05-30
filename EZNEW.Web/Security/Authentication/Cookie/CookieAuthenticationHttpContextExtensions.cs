using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Security.Authentication.Cookie
{
    public static class CookieAuthenticationHttpContextExtensions
    {
        /// <summary>
        /// 使用认证用户登陆
        /// </summary>
        /// <typeparam name="K">用户键值对象</typeparam>
        /// <param name="context">请求上下文</param>
        /// <param name="user">用户信息</param>
        /// <param name="properties">认证属性信息</param>
        /// <returns></returns>
        public static async Task SignInAsync<K>(this HttpContext context, AuthenticationUser<K> user, AuthenticationProperties properties = null)
        {
            var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaims(user.GetClaims());
            var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
            await context.SignInAsync(claimsPrincipal, properties).ConfigureAwait(false);
        }
    }
}
