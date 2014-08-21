using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiOAuthBase.Infrastructure.Interface;

namespace WebApiOAuthBase.Identity.DataModels
{
    public class ApplicationUser : IdentityUser, IAuditable
    {
        public virtual ICollection<UserClientAppAssignation> AuthorizedClientApplication { get; set; }

        public virtual ICollection<UserAuthorizationAssignation> UserAuthorizationAssignation { get; set; }

        #region Audit properties
        public DateTime CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public int CalenderID { get; set; }
        #endregion


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}