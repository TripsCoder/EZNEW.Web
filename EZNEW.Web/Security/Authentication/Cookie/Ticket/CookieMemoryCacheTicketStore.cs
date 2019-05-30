using EZNEW.Framework.Serialize;
using EZNEW.Web.Security.Authentication.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Security.Authentication.Cookie.Ticket
{
    public class CookieMemoryCacheTicketStore : IAuthenticationTicketStore
    {
        public CookieMemoryCacheTicketStore()
        {
        }

        public async Task<string> GetSessionTokenAsync(string subject)
        {
            var session = await CookieMemoryCacheSessionStore.GetSessionBySubjectAsync(subject).ConfigureAwait(false);
            return session?.SessionToken ?? string.Empty;
        }

        public async Task RemoveAsync(string key)
        {
            await CookieMemoryCacheSessionStore.DeleteSessionAsync(key).ConfigureAwait(false);
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var session = AuthSession.FromAuthenticationTicket(ticket);
            if (session == null)
            {
                await Task.CompletedTask;
            }
            session.SessionId = key;
            await CookieMemoryCacheSessionStore.StoreSessionAsync(session).ConfigureAwait(false);
        }

        public async Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var session = await CookieMemoryCacheSessionStore.GetSessionAsync(key).ConfigureAwait(false);
            if (session == null)
            {
                return null;
            }
            return session.ConvertToTicket();
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = Guid.NewGuid().ToString("N");
            await RenewAsync(key, ticket).ConfigureAwait(false);
            return key;
        }

        public async Task<bool> VerifyTicketAsync(string subject,string sessionToken, bool renew = true)
        {
            return await CookieMemoryCacheSessionStore.VerifySessionAsync(subject,sessionToken, renew).ConfigureAwait(false);
        }
    }
}
