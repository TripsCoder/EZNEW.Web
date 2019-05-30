using EZNEW.Framework.Extension;
using EZNEW.Framework.IoC;
using EZNEW.Framework.Net;
using EZNEW.Framework.Net.Http;
using EZNEW.Framework.Serialize;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Security.Authorization
{
    /// <summary>
    /// Authorize Manager
    /// </summary>
    public static class AuthorizeManager
    {
        /// <summary>
        /// authorize config
        /// </summary>
        static AuthorizeConfig authorizeConfig;

        static AuthorizeManager()
        {
            var authorizeConfigInfo = ContainerManager.Resolve<IOptions<AuthorizeConfig>>()?.Value ?? new AuthorizeConfig();
            authorizeConfig = authorizeConfigInfo;
        }

        /// <summary>
        /// get or set authorize verify method
        /// </summary>
        public static Func<AuthorizeVerifyRequest, Task<AuthorizeVerifyResult>> AuthorizeVerifyProcessAsync
        {
            get; set;
        }

        /// <summary>
        /// Authorize Verify
        /// </summary>
        /// <param name="verifyRequest">verify request</param>
        /// <returns></returns>
        public static async Task<AuthorizeVerifyResult> AuthorizeVerifyAsync(AuthorizeVerifyRequest verifyRequest)
        {
            if (verifyRequest == null)
            {
                return AuthorizeVerifyResult.ForbidResult();
            }
            if (!authorizeConfig.RemoteAuthorizeVerify)
            {
                if (AuthorizeVerifyProcessAsync == null)
                {
                    throw new ArgumentNullException(nameof(AuthorizeVerifyProcessAsync));
                }
                return (await AuthorizeVerifyProcessAsync(verifyRequest).ConfigureAwait(false)) ?? AuthorizeVerifyResult.ForbidResult();
            }
            string server = authorizeConfig.GetRandomServer();
            if (server.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(authorizeConfig.Servers));
            }
            var result = await HttpUtil.HttpPostJsonAsync(server, verifyRequest).ConfigureAwait(false);
            var stringValue = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            AuthorizeVerifyResult verifyResult = JsonSerialize.JsonToObject<AuthorizeVerifyResult>(stringValue);
            return verifyResult ?? AuthorizeVerifyResult.ForbidResult();
        }
    }
}
