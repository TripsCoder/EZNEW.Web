using System;
using System.Collections.Generic;
using System.Text;

namespace EZNEW.Web.Api.Request
{
    public class PagingQueryRequest : ApiRequest
    {
        /// <summary>
        /// Page Index
        /// </summary>
        public int Page
        {
            get; set;
        }

        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize
        {
            get; set;
        }
    }
}
