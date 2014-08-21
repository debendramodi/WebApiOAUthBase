using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiOAuthBase.Identity.DataAccess;
using WebApiOAuthBase.Identity.DataModels;
using WebApiOAuthBase.Infrastructure;
using WebApiOAuthBase.Infrastructure.Models;


namespace WebApiOAuthBase.Identity.Migrations
{
    public static class Seeder
    {
        public static void SeedAdminUser(SecurityDataContext db)
        {

            // 2 managers sharing the same DBContext coming from Seed methde implementation
            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            SecurityConfigurationManager ConfigManager = new SecurityConfigurationManager(db);

            const string name = AppConfig.AdminName;
            const string mail = AppConfig.AdminEmail;
            const string password = AppConfig.AdminPassword;


            var user = userManager.FindByName(name);
            if (user != null)
            {

                userManager.Delete(user);

            }

            user = new ApplicationUser { UserName = name, Email = mail };
            user.EmailConfirmed = true;
            IdentityResult result = userManager.Create(user, password);
            result = userManager.SetLockoutEnabled(user.Id, false);

            // Seed admin Authorizations
            ApplicationAuthorization AdminAuth = new ApplicationAuthorization();
            AdminAuth.Code = AuthorizationCode.SuperAdmin;
            AdminAuth.Description = "All access from all client type";
            AdminAuth.Name = "SuperAdmin";
            ConfigManager.AddApplicationAuthorization(AdminAuth);


            //Seed default ClientApplications
            ClientApplication WPFApp = new ClientApplication();
            WPFApp.ApplicationDescription = "WPF client application";
            WPFApp.ApplicationCode = "WPF";
            WPFApp.ApplicationSecret = "WPF123";
            ConfigManager.AddClientApplication(WPFApp);
            userManager.AuthorizeApplicationtoClient(WPFApp, user, db);

            ClientApplication WebApp = new ClientApplication();
            WebApp.ApplicationDescription = "WEB client application";
            WebApp.ApplicationCode = "WEB";
            WebApp.ApplicationSecret = "WEB123";
            ConfigManager.AddClientApplication(WebApp);
            userManager.AuthorizeApplicationtoClient(WebApp, user, db);

            ClientApplication WStoreApp = new ClientApplication();
            WStoreApp.ApplicationDescription = "WStore client application";
            WStoreApp.ApplicationCode = "WStore";
            WStoreApp.ApplicationSecret = "WStore123";
            ConfigManager.AddClientApplication(WStoreApp);
            userManager.AuthorizeApplicationtoClient(WStoreApp, user, db);

            // Assign ClientAutorisation to user
            userManager.AssignAuthForClientAppToUser(AdminAuth, WPFApp, user, db);
            userManager.AssignAuthForClientAppToUser(AdminAuth, WebApp, user, db);
            userManager.AssignAuthForClientAppToUser(AdminAuth, WStoreApp, user, db);




        }
    }
}