using IdentityModel;
using EZNEW.Framework.ValueType;
using EZNEW.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace EZNEW.Web.Security.Authentication
{
    /// <summary>
    /// Authentication User 
    /// </summary>
    public class AuthenticationUser<TK> : IIdentity
    {
        #region Propertys

        /// <summary>
        /// user id
        /// </summary>
        public TK Id
        {
            get; set;
        }

        /// <summary>
        /// authentication type
        /// </summary>
        public string AuthenticationType
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// is authenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// name
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// real name
        /// </summary>
        public string RealName
        {
            get; set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// get user from principal
        /// </summary>
        /// <param name="principal">principal</param>
        /// <returns></returns>
        public static AuthenticationUser<TK> GetUserFromPrincipal(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }
            return GetUserFromClaims(principal.Claims);
        }

        /// <summary>
        /// get user from claims
        /// </summary>
        /// <param name="claims">claims</param>
        /// <returns></returns>
        public static AuthenticationUser<TK> GetUserFromClaims(IEnumerable<Claim> claims)
        {
            if (claims.IsNullOrEmpty())
            {
                return null;
            }
            var idClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                idClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject);
            }
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (nameClaim == null)
            {
                nameClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Name);
            }
            var realNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (realNameClaim == null)
            {
                realNameClaim = realNameClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.NickName);
            }
            if (idClaim == null)
            {
                return null;
            }
            return new AuthenticationUser<TK>()
            {
                Id = DataConverter.ConvertToSimpleType<TK>(idClaim.Value),
                Name = nameClaim?.Value,
                RealName = realNameClaim?.Value
            };
        }

        /// <summary>
        /// get claims
        /// </summary>
        /// <returns></returns>
        public virtual List<Claim> GetClaims()
        {
            return new List<Claim>()
            {
                new Claim(JwtClaimTypes.Subject,Id.ToString()),
                new Claim(JwtClaimTypes.Name,Name??string.Empty),
                new Claim(JwtClaimTypes.NickName,RealName??string.Empty)
            };
        }

        #endregion
    }
}
