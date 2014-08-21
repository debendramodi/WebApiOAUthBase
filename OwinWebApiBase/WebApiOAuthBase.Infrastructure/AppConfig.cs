using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebApiOAuthBase.Infrastructure
{
    public static class AppConfig
    {
        public const int TokenExiration = 8;
        public const string SecuritDatabaseConnectionString = "SecurityCS";
        public const string AdminEmail = "gaetan.jaminon@hotmai.com";
        public const string AdminPassword = "Aeson123";
        public const string AdminName = "admin";
        public const bool DropDatabaseAtStart = false;
        public const string DatabaseName = "AesonDevDatabase"; // Dont forget to change the name in The Web.Config. They must be the same
       
       
    }
}