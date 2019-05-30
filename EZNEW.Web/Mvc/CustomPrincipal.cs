using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Mvc
{
    public class CustomPrincipal : ClaimsPrincipal
    {
        public CustomPrincipal(IIdentity identity):base(identity)
        {
        }

        public override bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
