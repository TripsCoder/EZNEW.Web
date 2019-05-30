using IdentityModel;
using EZNEW.Framework.IoC;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EZNEW.Web.Security.Authentication.Session
{
    /// <summary>
    /// Session
    /// </summary>
    public class AuthSession
    {
        public AuthSession()
        {
            SessionToken= Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 授权方式
        /// </summary>
        [JsonProperty(PropertyName = "authentication_scheme")]
        public string AuthenticationScheme
        {
            get; set;
        }

        /// <summary>
        /// 身份信息
        /// </summary>
        [JsonProperty(PropertyName = "claims")]
        public Dictionary<string, string> Claims
        {
            get; set;
        }

        /// <summary>
        /// 身份认证属性值
        /// </summary>
        [JsonProperty(PropertyName = "properties_items")]
        public Dictionary<string, string> PropertiesItems
        {
            get; set;
        }

        /// <summary>
        /// 过期时间
        /// </summary>
        [JsonProperty(PropertyName = "expires")]
        public DateTimeOffset Expires
        {
            get; set;
        }

        /// <summary>
        /// session token
        /// </summary>
        [JsonProperty(PropertyName = "session_token")]
        public string SessionToken
        {
            get; set;
        }

        [JsonProperty(PropertyName = "session_id")]
        public string SessionId
        {
            get;set;
        }

        /// <summary>
        /// 获取用户编号
        /// </summary>
        /// <returns></returns>
        public string GetSubjectId()
        {
            if (Claims == null || Claims.Count <= 0)
            {
                return string.Empty;
            }
            if (Claims.ContainsKey(ClaimTypes.NameIdentifier))
            {
                return Claims[ClaimTypes.NameIdentifier];
            }
            if (Claims.ContainsKey(JwtClaimTypes.Subject))
            {
                return Claims[JwtClaimTypes.Subject];
            }
            return string.Empty;
        }

        /// <summary>
        /// AuthenticationTicket转换为Session对象
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static AuthSession FromAuthenticationTicket(AuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                return null;
            }
            var session = new AuthSession()
            {
                AuthenticationScheme = ticket.AuthenticationScheme,
                PropertiesItems = ticket.Properties.Items.ToDictionary(c => c.Key, c => c.Value),
                Claims = ticket.Principal.Claims?.ToDictionary(c => c.Type, c => c.Value),
            };
            var sessionConfig = SessionConfig.GetSessionConfig();
            if (session.Claims?.ContainsKey(sessionConfig.SessionClaimName)??false)
            {
                session.SessionToken = session.Claims[sessionConfig.SessionClaimName];
            }
            session.Expires = DateTimeOffset.Now.Add(sessionConfig.Expires);
            return session;
        }

        /// <summary>
        /// 将session对象转换为Ticket
        /// </summary>
        /// <returns></returns>
        public AuthenticationTicket ConvertToTicket()
        {
            var claimIdentity = new ClaimsIdentity(AuthenticationScheme);
            claimIdentity.AddClaims(Claims.Select(c => new Claim(c.Key, c.Value)));
            var sessionConfig = SessionConfig.GetSessionConfig();
            var nowSessionClaim = claimIdentity.Claims.FirstOrDefault(c => c.Type == sessionConfig.SessionClaimName);
            if (nowSessionClaim == null)
            {
                claimIdentity.AddClaim(new Claim(sessionConfig.SessionClaimName, SessionToken));
            }
            AuthenticationProperties props = new AuthenticationProperties();
            if (PropertiesItems != null)
            {
                foreach (var pitem in PropertiesItems)
                {
                    props.Items.Add(pitem.Key, pitem.Value);
                }
            }
            props.IsPersistent = true;
            props.AllowRefresh = true;
            return new AuthenticationTicket(new ClaimsPrincipal(claimIdentity), props, AuthenticationScheme);
        }

        /// <summary>
        /// 获取session键值
        /// </summary>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static string GetSessionToken(Dictionary<string, string> values)
        {
            if (values == null || values.Count <= 0)
            {
                return string.Empty;
            }
            var sessionConfig = SessionConfig.GetSessionConfig();
            values.TryGetValue(sessionConfig.SessionClaimName, out string sessionToken);
            return sessionToken;
        }

        /// <summary>
        /// 获取session键值
        /// </summary>
        /// <param name="claims">凭据信息</param>
        /// <returns></returns>
        public static string GetSessionToken(IEnumerable<Claim> claims)
        {
            if (claims != null && claims.Any())
            {
                var sessionConfig = SessionConfig.GetSessionConfig();
                var sessionClaim = claims.FirstOrDefault(c => c.Type == sessionConfig.SessionClaimName);
                return sessionClaim?.Value ?? string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取授权对象
        /// </summary>
        /// <param name="values">值</param>
        /// <returns></returns>
        public static string GetSubject(Dictionary<string, string> values)
        {
            if (values == null || values.Count <= 0)
            {
                return string.Empty;
            }
            if (values.ContainsKey(ClaimTypes.NameIdentifier))
            {
                return values[ClaimTypes.NameIdentifier];
            }
            if (values.ContainsKey(JwtClaimTypes.Subject))
            {
                return values[JwtClaimTypes.Subject];
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取授权对象
        /// </summary>
        /// <param name="claims">凭据信息</param>
        /// <returns></returns>
        public static string GetSubject(IEnumerable<Claim> claims)
        {
            if (claims != null && claims.Any())
            {
                var sessionConfig = SessionConfig.GetSessionConfig();
                return GetSubject(claims.ToDictionary(c => c.Type, c => c.Value));
            }
            return string.Empty;
        }

        /// <summary>
        /// 验证session是否可用
        /// </summary>
        /// <param name="sessionId">session id</param>
        /// <returns></returns>
        public bool AllowUse(string sessionId = "",string sessionToken="")
        {
            var success = DateTimeOffset.Now <= Expires;
            if (success && !string.IsNullOrWhiteSpace(sessionId))
            {
                success=SessionId == sessionId;
            }
            if (success && !string.IsNullOrWhiteSpace(sessionToken))
            {
                success = SessionToken == sessionToken;
            }
            return success;
        }
    }
}
