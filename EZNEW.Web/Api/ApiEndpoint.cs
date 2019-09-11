using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Api
{
    /// <summary>
    /// api endpoint
    /// </summary>
    public class ApiEndpoint
    {
        /// <summary>
        /// endpoint name
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// endpoint path
        /// </summary>
        public string Path
        {
            get; set;
        }
    }
}
