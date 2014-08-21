using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiOAuthBase.Infrastructure.Interface;


namespace WebApiOAuthBase.Identity.DataModels
{
    public class ClientApplication : IAuditable
    {
        public int id { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationSecret { get; set; }

        public virtual ICollection<UserClientAppAssignation> UserAuthorized { get; set; }
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