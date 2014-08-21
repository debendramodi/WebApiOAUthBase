using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApiOwinBase.Providers
{
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            // refresh tokens don't expire
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddHours(3);
            context.SetToken(context.SerializeTicket());
        }

        public override async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}