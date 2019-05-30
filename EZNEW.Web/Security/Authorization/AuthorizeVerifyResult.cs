using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Security.Authorization
{
    /// <summary>
    /// Authorize Verify Result
    /// </summary>
    public class AuthorizeVerifyResult
    {
        /// <summary>
        /// Verify Value
        /// </summary>
        public AuthorizeVerifyValue VerifyValue
        {
            get; set;
        }

        /// <summary>
        /// Allow Access
        /// </summary>
        public bool AllowAccess
        {
            get
            {
                return VerifyValue == AuthorizeVerifyValue.Success;
            }
        }

        /// <summary>
        /// Challenge
        /// </summary>
        /// <returns></returns>
        public static AuthorizeVerifyResult ChallengeResult()
        {
            return GetAuthorizeVerifyResult(AuthorizeVerifyValue.Challenge);
        }

        /// <summary>
        /// Forbid
        /// </summary>
        /// <returns></returns>
        public static AuthorizeVerifyResult ForbidResult()
        {
            return GetAuthorizeVerifyResult(AuthorizeVerifyValue.Forbid);
        }

        /// <summary>
        /// Success
        /// </summary>
        /// <returns></returns>
        public static AuthorizeVerifyResult SuccessResult()
        {
            return GetAuthorizeVerifyResult(AuthorizeVerifyValue.Success);
        }

        /// <summary>
        /// Get Authorize Verify Result
        /// </summary>
        /// <param name="value">result type</param>
        /// <returns></returns>
        public static AuthorizeVerifyResult GetAuthorizeVerifyResult(AuthorizeVerifyValue value = AuthorizeVerifyValue.Forbid)
        {
            return new AuthorizeVerifyResult()
            {
                VerifyValue = value
            };
        }
    }

    public enum AuthorizeVerifyValue
    {
        Challenge = 110, //未登陆
        Forbid = 120, //未授权
        Success = 130 //成功
    }
}
