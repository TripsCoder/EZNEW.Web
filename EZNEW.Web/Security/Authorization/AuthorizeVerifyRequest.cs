using EZNEW.Framework.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Security.Authorization
{
    /// <summary>
    /// Authorize Verify Request
    /// </summary>
    public class AuthorizeVerifyRequest
    {
        /// <summary>
        /// Controller
        /// </summary>
        public string ControllerCode
        {
            get; set;
        }

        /// <summary>
        /// Action
        /// </summary>
        public string ActionCode
        {
            get; set;
        }

        /// <summary>
        /// Application
        /// </summary>
        public ApplicationInfo Application
        {
            get; set;
        }

        /// <summary>
        /// Claims
        /// </summary>
        public Dictionary<string, string> Claims
        {
            get; set;
        }
    }
}
