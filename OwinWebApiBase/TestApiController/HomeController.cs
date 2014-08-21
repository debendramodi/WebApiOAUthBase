using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiOAuthBase.Infrastructure.Models;
using WebApiOAuthBase.Infrastructure.OAuth2;


namespace TestApiController
{
    [Authorize]
    public class HomeController : ApiController
    {
        [AesonActionDemand(AuthorizationCode.SuperAdmin)]
        public int[] GetTest()
        {
            IPrincipal u = Thread.CurrentPrincipal;

            return new[] { 1, 2, 3, 3, 5, 6 };
        }
    }
}
