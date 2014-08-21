using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApiOwinBase.Providers
{
    public class AuthorizationCodeProvider : AuthenticationTokenProvider
    {
        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            context.SetToken(context.SerializeTicket());
        }

        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}