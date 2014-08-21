using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiOAuthBase.Infrastructure.Interface;

namespace WebApiOAuthBase.Identity.DataModels
{
    public class UserAuthorizationAssignation : IAuditable
    {
        public int id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationAuthorization Authorization { get; set; }
        public virtual ClientApplication ClientApplication { get; set; }

        #region Audit properties
        public DateTime CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public int CalenderID { get; set; }
        #endregion
    }
}