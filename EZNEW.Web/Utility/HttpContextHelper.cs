using EZNEW.Framework.IoC;
using EZNEW.Web.DI;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Utility
{
    public static class HttpContextHelper
    {
        /// <summary>
        /// current context
        /// </summary>
        public static HttpContext Current
        {
            get
            {
                object factory = ContainerManager.Resolve<IHttpContextAccessor>();
                HttpContext context = ((HttpContextAccessor)factory).HttpContext;
                return context;
            }
        }
    }
}
