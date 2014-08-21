using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiOAuthBase.Infrastructure.Interface
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        String CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        String UpdatedBy { get; set; }

        // Save all event in a calendar to see all event per date
        int CalenderID { get; set; }

    }
}
