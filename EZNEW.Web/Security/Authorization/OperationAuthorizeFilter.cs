using EZNEW.Framework.Application;
using EZNEW.Framework.IoC;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Security.Authorization
{
    public class OperationAuthorizeFilter : AuthorizeFilter
    {
        private static bool HasAllowAnonymous(IList<IFilterMetadata> filters)
        {
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }

            return false;
        }

        public override async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var task = base.OnAuthorizationAsync(context);
            if (context.Result != null && (context.Result is ChallengeResult || context.Result is ForbidResult))
            {
                return;
            }
            if (HasAllowAnonymous(context.Filters))//allow anonymous access
            {
                return;
            }
            bool isAuthenticated = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                context.Result = new ChallengeResult();
                return;
            }
            var verifyResult = await AuthorizeManager.AuthorizeVerifyAsync(new AuthorizeVerifyRequest()
            {
                ControllerCode = context.RouteData.Values["controller"]?.ToString().ToUpper() ?? string.Empty,
                ActionCode = context.RouteData.Values["action"]?.ToString().ToUpper() ?? string.Empty,
                Application = ApplicationManager.Current,
                Claims = context.HttpContext.User?.Claims?.ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>(0)
            }).ConfigureAwait(false);
            switch (verifyResult.VerifyValue)
            {
                case AuthorizeVerifyValue.Challenge:
                    context.Result = new ChallengeResult();
                    break;
                case AuthorizeVerifyValue.Forbid:
                default:
                    context.Result = new ForbidResult();
                    break;
                case AuthorizeVerifyValue.Success:
                    break;
            }
        }
    }
}
