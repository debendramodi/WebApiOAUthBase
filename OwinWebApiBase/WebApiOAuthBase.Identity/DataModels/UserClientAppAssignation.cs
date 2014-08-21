using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiOAuthBase.Infrastructure.Interface;


namespace WebApiOAuthBase.Identity.DataModels
{
    public class UserClientAppAssignation: IAuditable
    {
        public int id { get; set; }
        public bool Blocked { get; set; }
        public string BlockingDate { get; set; }
        public string UserBlockingName { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual ClientApplication App { get; set; }

        #region Audit properties
        public DateTime CreatedDate { get; set; }
        public String CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public int CalenderID { get; set; }
        #endregion




    }
}