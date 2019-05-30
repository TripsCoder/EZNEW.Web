using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.Web.Config.App
{
    /// <summary>
    /// Application Type
    /// </summary>
    public enum ApplicationType
    {
        WebApplication = 110,
        WebService = 120
    }

    /// <summary>
    /// Application Status
    /// </summary>
    public enum ApplicationStatus
    {
        run = 210,
        closed = 220
    }
}
