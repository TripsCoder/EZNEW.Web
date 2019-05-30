using IdentityModel;
using EZNEW.Framework.Extension;
using EZNEW.Web.Security.Authentication.Session;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Security.Authentication.Cookie.Ticket
{
    /// <summary>
    /// session存储
    /// </summary>
    public static class CookieMemoryCacheSessionStore
    {
        private static IMemoryCache _cache;

        static CookieMemoryCacheSessionStore()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        #region 存储Session

        /// <summary>
        /// 存储Session
        /// </summary>
        /// <param name="authSession">session对象</param>
        /// <returns></returns>
        public static async Task StoreSessionAsync(AuthSession authSession)
        {
            if (authSession == null)
            {
                throw new ArgumentNullException(nameof(authSession));
            }
            string subjectId = authSession.GetSubjectId();
            if (string.IsNullOrWhiteSpace(subjectId))
            {
                throw new Exception("authentication subject is null or empty");
            }
            string sessionId = authSession.SessionId;
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new Exception("session id is null or empty");
            }
            var expiresSeconds = (authSession.Expires - DateTimeOffset.Now).TotalSeconds;
            var options = new MemoryCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromSeconds(expiresSeconds));
            _cache.Set(sessionId, subjectId, options);
            _cache.Set(subjectId, authSession, options);
            await Task.CompletedTask.ConfigureAwait(false);
        }

        #endregion

        #region 删除Session

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="sessionId">session键值</param>
        /// <returns></returns>
        public static async Task DeleteSessionAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                await Task.CompletedTask;
            }
            var session = await GetSessionAsync(sessionId).ConfigureAwait(false);
            _cache.Remove(sessionId);
            if (session != null)
            {
                _cache.Remove(session.GetSubjectId());
            }
            await Task.CompletedTask.ConfigureAwait(false);
        }

        #endregion

        #region 获取Session

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="sessionId">session key</param>
        /// <returns></returns>
        public static async Task<AuthSession> GetSessionAsync(string sessionId)
        {
            if (sessionId.IsNullOrEmpty())
            {
                return null;
            }
            var subject = _cache.Get<string>(sessionId);
            if (subject.IsNullOrEmpty())
            {
                return null;
            }
            var session = await GetSessionBySubjectAsync(subject).ConfigureAwait(false);
            if (!(session?.AllowUse(sessionId) ?? false))
            {
                _cache.Remove(sessionId);
                session = null;
            }
            return session;
        }

        /// <summary>
        /// 根据登陆账号身份编号获取session
        /// </summary>
        /// <param name="subject">身份编号</param>
        /// <returns></returns>
        public static async Task<AuthSession> GetSessionBySubjectAsync(string subject)
        {
            if (subject == null)
            {
                return null;
            }
            _cache.TryGetValue(subject, out AuthSession session);
            if (!(session?.AllowUse() ?? false))
            {
                session = null;
            }
            return await Task.FromResult(session).ConfigureAwait(false);
        }

        #endregion

        #region 验证Session

        /// <summary>
        /// 验证Session是否有效
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="sessionToken">session key</param>
        /// <param name="refresh">refresh session</param>
        /// <returns></returns>
        public static async Task<bool> VerifySessionAsync(string subject, string sessionToken, bool refresh = true)
        {
            if (string.IsNullOrWhiteSpace(sessionToken) || string.IsNullOrWhiteSpace(subject))
            {
                return false;
            }
            var session = await GetSessionBySubjectAsync(subject).ConfigureAwait(false);
            var verifySuccess = session?.AllowUse(sessionToken: sessionToken) ?? false;
            if (verifySuccess && refresh)
            {
                await StoreSessionAsync(session).ConfigureAwait(false);
            }
            return verifySuccess;
        }

        /// <summary>
        /// 验证Session凭据是否有效
        /// </summary>
        /// <param name="claims">凭据</param>
        /// <returns></returns>
        public static async Task<bool> VerifySessionAsync(Dictionary<string, string> claims, bool refresh = true)
        {
            if (claims == null || claims.Count <= 0)
            {
                return false;
            }
            var sessionToken = AuthSession.GetSessionToken(claims);
            var subject = AuthSession.GetSubject(claims);
            return await VerifySessionAsync(subject, sessionToken, refresh).ConfigureAwait(false);
        }

        #endregion
    }
}
