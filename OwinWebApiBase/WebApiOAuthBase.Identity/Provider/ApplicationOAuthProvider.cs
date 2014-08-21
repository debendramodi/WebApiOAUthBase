using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebApiOAuthBase.Identity.DataAccess;


namespace WebApiOAuthBase.Identity.Provider
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {       
        private OAuthDataManager DataManager;
        public ApplicationOAuthProvider()
        {
            DataManager = new OAuthDataManager();
        }

        // Validate Client Application
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {           
            string ApplicationTypeCode = "";
            string AppSecret = "";
            string UserName = "";
            if (context.TryGetBasicCredentials(out ApplicationTypeCode, out AppSecret))
            {
               int ClientApplicationID = DataManager.ValidateClientApplication(ApplicationTypeCode, AppSecret);
               if (ClientApplicationID != -1)
               {                  
                   if (context.Parameters.Count(p => p.Key == "username") != 0)
                   {
                       UserName = context.Parameters.First(p => p.Key == "username").Value.FirstOrDefault();
                       bool b = DataManager.ValidateApplicationForUser(ClientApplicationID, UserName);
                       if (b)
                       {                           
                           context.Validated();
                       }
                       else
                       {
                           // Application not valide for this UserName
                           context.Rejected();
                       }
                   }
                   else
                   {
                       //UserName value not found in headers
                       context.Rejected();
                   }
                
               }
               else
               {
                   // Combinaison CLientID/CLientPassword incorect
                   context.Rejected();
               }
            }
            else
            {
                // ClientID or ClientSecret not found in headers
                context.Rejected();
            }         
            return Task.FromResult(0);
          
        }

        //Validate User
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string UserName = context.UserName;
            string UserSecret = context.Password;
            string UserID = "-1";

            //ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user, context.Options.AuthenticationType);            
            //ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user, CookieAuthenticationDefaults.AuthenticationType);
            //AuthenticationProperties properties = CreateProperties(user.UserName);

            //AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            //context.Validated(ticket);
            //context.Request.Context.Authentication.SignIn(cookiesIdentity);
            UserID = DataManager.CheckUserSecret(UserName,UserSecret);
            if (UserID != "-1")
            {

                ClaimsIdentity id = DataManager.GetClaimsFromUserID(UserID, context);
                context.Validated(id);
            }
            else
            {
                // UserNotFound
                context.Rejected();

            }


            return Task.FromResult(0);
          
        }


       
     
    }
}