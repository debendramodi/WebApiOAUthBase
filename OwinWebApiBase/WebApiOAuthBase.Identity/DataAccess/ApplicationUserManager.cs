using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using WebApiOAuthBase.Infrastructure.Models;
using WebApiOAuthBase.Identity.DataModels;

namespace WebApiOAuthBase.Identity.DataAccess
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store): base(store)
        {
        
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, 
                                                                                            IOwinContext context)
        {
           

            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<SecurityDataContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        internal BaseActionReturnModel AssignAuthForClientAppToUser(ApplicationAuthorization Authorization,
                                                                        ClientApplication ClientApp,
                                                                           ApplicationUser User,
                                                                                SecurityDataContext ctx)
        {          
            try
            {

                // Prevent double authorisation
                if (ctx.UserAuthorizationAssignations.Count(p => p.ClientApplication.id == ClientApp.id
                                                                    && p.Authorization.id == Authorization.id
                                                                       && p.User.Id == User.Id) == 0)
                {
                    UserAuthorizationAssignation r = new UserAuthorizationAssignation();
                    r.Authorization = Authorization;
                    r.User = User;
                    r.ClientApplication = ClientApp;
                    ctx.UserAuthorizationAssignations.Add(r);
                    return BaseActionReturnModel.CreateSuccededResult("OK", false, null, false);
                }
                else
                {
                    return BaseActionReturnModel.CreateSuccededResult("Authorization for application already assigned to client", false, null, true);
                }

             
              

               
            }
            catch (Exception exc)
            {
                return BaseActionReturnModel.CreateException(exc, false);
            }

        }



        internal BaseActionReturnModel AuthorizeApplicationtoClient(ClientApplication ClientApp,
                                                                        ApplicationUser User,
                                                                            SecurityDataContext ctx)
        {
            try
            {
                // Prevent double assignation
                if (ctx.UserAuthorizationAssignations.Count(p => p.ClientApplication.id == ClientApp.id
                                                                       && p.User.Id == User.Id) == 0)
                {
                    UserClientAppAssignation r = new UserClientAppAssignation();
                    r.User = User;
                    r.Blocked = false;
                    r.App = ClientApp;
                    ctx.UserClientAppAssignations.Add(r);
                    return BaseActionReturnModel.CreateSuccededResult("OK", false, null,false);
                }
                else
                {
                    return BaseActionReturnModel.CreateSuccededResult("Application already assigned to client", false, null,true);
                }


            }
            catch (Exception exc)
            {
                return BaseActionReturnModel.CreateException(exc, false);
            }
        }


    }

   
}