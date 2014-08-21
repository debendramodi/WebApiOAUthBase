using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiOAuthBase.Infrastructure.Models;
using WebApiOAuthBase.Infrastructure.Interface;

namespace WebApiOAuthBase.Identity.DataModels
{
    public class ApplicationAuthorization : IAuditable
    {
        public int id { get; set; }
        public string Name { get; set; }
        public AuthorizationCode Code { get; set; }       
        public string Description { get; set; }

        public virtual ICollection<UserAuthorizationAssignation> UserAuthorizationAssignation { get; set; }

        #region Audit properties
        public DateTime CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public int CalenderID { get; set; }
        #endregion
    }
}