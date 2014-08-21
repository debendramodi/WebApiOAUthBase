using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using WebApiOAuthBase.Identity.DataModels;

namespace WebApiOAuthBase.Identity.DataAccess
{
    public class OAuthDataManager
    {
        // Called before WebApi initialization
        // Do not create DbContext in constructor because constructor is only created at application start
        // but each validation is called when a client get a token. We need to get a fresh db context for each token asked
        // and each login acces       
        public OAuthDataManager()
        {

        }
        //Return the ApplicationID if exist        
        internal int ValidateClientApplication(string ApplicationTypeCode, string AppSecret)
        {
            using (SecurityDataContext db = new SecurityDataContext())
            {
                int i = db.ClientApplications.Count(p => p.ApplicationCode == ApplicationTypeCode 
                                                            && p.ApplicationSecret == AppSecret);
                if (i != 0)
                {
                    int AppID = db.ClientApplications.First(p => p.ApplicationCode == ApplicationTypeCode
                                                            && p.ApplicationSecret == AppSecret).id;
                    return AppID;
                }
                else
                {
                    return -1;
                }               
            }
        }

        internal bool ValidateApplicationForUser(int AppID, string UserName)
        {
            using (SecurityDataContext db = new SecurityDataContext())
            {
               int i = db.UserClientAppAssignations.Count(p => p.App.id == AppID && p.User.UserName == UserName);
               if (i == 0)
               {
                   return false;
               }
               else
               {
                   return true;
               }
            }
        }

        internal string CheckUserSecret(string UserName, string UserSecret)
        {
            // return UserID if existe else return -1
          
            using (SecurityDataContext db = new SecurityDataContext())
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                ApplicationUser u = manager.Find(UserName, UserSecret);
                if (u != null)
                {
                    return u.Id;
                }
                else
                {
                    return "-1";
                }
            }
        }

        internal ClaimsIdentity GetClaimsFromUserID(string UserID, OAuthGrantResourceOwnerCredentialsContext context)
        {
            // create identity
            ClaimsIdentity id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim("UserName", context.UserName));
            id.AddClaim(new Claim("UserID", UserID.ToString()));
            id.AddClaim(new Claim("CurentAppTypeCode", context.ClientId));
            // Get AUthorization from database
            using (SecurityDataContext db = new SecurityDataContext())
            {
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
                ApplicationUser u = manager.FindById(UserID);
                foreach (var item in u.UserAuthorizationAssignation)
                {
                    // ne prendre en compte que les autorisation corespondant au type d'application que le client est en train d'utiliser
                    if (item.ClientApplication.ApplicationCode == context.ClientId)
                    {
                        id.AddClaim(new Claim("Auth", item.Authorization.Code.ToString()));
                    }
                }
               
            }
            return id;
        }
    }
}