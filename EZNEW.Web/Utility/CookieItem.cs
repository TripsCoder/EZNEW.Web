using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Utility
{
    public class CookieItem
    {
        public string Key
        { get; set; }

        public string Value
        { get; set; }

        public CookieOptions Options
        {
            get;set;
        }
    }
}
