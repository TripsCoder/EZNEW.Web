using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Utility
{
    /// <summary>
    /// cookie item
    /// </summary>
    public class CookieItem
    {
        /// <summary>
        /// cookie key
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// cookie value
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        ///cookie options
        /// </summary>
        public CookieOptions Options
        {
            get;set;
        }
    }
}
