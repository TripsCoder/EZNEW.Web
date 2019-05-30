using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Security.Authentication.Cookie
{
    public class CookieAuthenticationEventHandler : CookieAuthenticationEvents
    {
        /// <summary>
        /// 是否强制执行凭据验证
        /// </summary>
        internal static bool ForceValidatePrincipal
        {
            get; set;
        } = false;

        /// <summary>
        /// 验证Cookie凭据方法
        /// </summary>
        internal static Func<CookieValidatePrincipalContext, Task<bool>> OnValidatePrincipalAsync
        {
            get; set;
        }

        /// <summary>
        /// 执行Cookie凭据验证
        /// </summary>
        /// <param name="context">凭据验证上下文信息</param>
        /// <returns></returns>
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            if (OnValidatePrincipalAsync == null)
            {
                if (ForceValidatePrincipal)
                {
                    context.RejectPrincipal();
                }
                context.ShouldRenew = true;
            }
            else
            {
                var result = await OnValidatePrincipalAsync(context).ConfigureAwait(false);
                if (!result)
                {
                    context.RejectPrincipal();
                }
                else
                {
                    context.ShouldRenew = true;
                }
            }
        }
    }
}
