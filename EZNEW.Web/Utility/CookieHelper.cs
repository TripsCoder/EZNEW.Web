using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EZNEW.Framework.Security;
using Microsoft.AspNetCore.Http;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// Cookie Helper
    /// </summary>
    public static class CookieHelper
    {
        #region Save Cookie

        /// <summary>
        /// Save Cookie
        /// </summary>
        /// <param name="cookie">cookie object</param>
        public static void SaveCookie(CookieItem cookie)
        {
            if (cookie == null)
            {
                return;
            }
            cookie.Options = cookie.Options ?? new CookieOptions();
            cookie.Options.HttpOnly = true;

            HttpContextHelper.Current.Response.Cookies.Append(cookie.Key, cookie.Value, cookie.Options);
        }

        #endregion

        #region Get Cookie By Name

        /// <summary>
        /// get cookie by name
        /// </summary>
        /// <param name="cookieName">name</param>
        /// <returns>cookie object</returns>
        public static CookieItem GetCookie(string cookieName)
        {
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                return null;
            }
            var cookieCollection = HttpContextHelper.Current.Request.Cookies;
            if (cookieCollection.Keys.Contains(cookieName))
            {
                return new CookieItem()
                {
                    Key = cookieName,
                    Value = cookieCollection[cookieName]
                };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Get Value By Name

        /// <summary>
        /// get value by name
        /// </summary>
        /// <param name="cookieName">name</param>
        /// <returns>value</returns>
        public static string GetCookieValue(string cookieName)
        {
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                return string.Empty;
            }
            var nowCookie = GetCookie(cookieName);
            if (nowCookie == null)
            {
                return string.Empty;
            }
            return nowCookie.Value;
        }

        #endregion

        #region Set Cookie Value

        /// <summary>
        /// set cookie value
        /// </summary>
        /// <param name="cookieName">name</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static bool SetCookieValue(string cookieName, string value, DateTime? expiresTime = null)
        {
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                return false;
            }
            var nowCookie = GetCookie(cookieName);
            if (nowCookie == null)
            {
                nowCookie = new CookieItem()
                {
                    Key = cookieName
                };
            }
            if (!expiresTime.HasValue)
            {
                expiresTime = DateTime.Now.AddHours(2);
            }
            var options = nowCookie.Options ?? new CookieOptions();
            nowCookie.Value = value;
            options.Expires = expiresTime.Value;
            nowCookie.Options = options;
            SaveCookie(nowCookie);
            return true;
        }

        #endregion

        #region Remove Cookie By Name

        /// <summary>
        /// remove cookie by name
        /// </summary>
        /// <param name="cookieName">name</param>
        /// <returns></returns>
        public static bool RemoveCookie(string cookieName)
        {
            if (string.IsNullOrWhiteSpace(cookieName))
            {
                return false;
            }
            HttpContextHelper.Current.Response.Cookies.Delete(cookieName);
            return true;
        }

        #endregion
    }
}
