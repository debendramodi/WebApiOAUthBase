using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web;
using System.Security.Principal;
using System.Threading;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.WebApi;
using Microsoft.Owin.Logging;
using System.Web.Http.Dispatcher;
using WebApiOAuthBase.Infrastructure.OAuth2;
using WebApiOAuthBase.Infrastructure;
using WebApiOAuthBase.Identity.Provider;
using WebApiOAuthBase.Identity.DataAccess;
using WebApiOAuthBase.Identity.DataModels;
using WebApiOAuthBase.Identity;

[assembly: OwinStartup(typeof(WebApiOAuthBase.Web.Startup))]

namespace WebApiOAuthBase.Web
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            #region Register object per OwinContext
            app.CreatePerOwinContext(SecurityDataContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            #endregion

            #region register AutorizationProvider
            ClaimsAuthorization.CustomAuthorizationManager = new ClaimsAuthorizationProvider();
            #endregion

            #region  token generation

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(AppConfig.TokenExiration),
                Provider = new ApplicationOAuthProvider()
            });
            #endregion

            #region token consumption
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            #endregion

            #region WebApi MiddelWare
            HttpConfiguration config = new HttpConfiguration();
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("default","{controller}"); 
            app.UseWebApi(config);
            #endregion

           
        }

      
    }

   
}
