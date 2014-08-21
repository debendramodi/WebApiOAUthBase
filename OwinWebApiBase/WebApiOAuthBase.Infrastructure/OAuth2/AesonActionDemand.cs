using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Thinktecture.IdentityModel;
using WebApiOAuthBase.Infrastructure.Models;

namespace WebApiOAuthBase.Infrastructure.OAuth2
{
    public class AesonActionDemand : AuthorizeAttribute
    {
        private AuthorizationCode Authorization;
        // notimplemented
        private string[] _resources;

        public AesonActionDemand()
        { }

        public AesonActionDemand(AuthorizationCode Authorization)
        {
            this.Authorization = Authorization;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return CheckAccess(actionContext);
        }

        protected virtual bool CheckAccess(HttpActionContext actionContext)
        {
            var action = actionContext.ActionDescriptor.ActionName;
            var resource = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            return ClaimsAuthorization.CheckAccess(
                Authorization.ToString(),
                resource);
        }
    }
}
