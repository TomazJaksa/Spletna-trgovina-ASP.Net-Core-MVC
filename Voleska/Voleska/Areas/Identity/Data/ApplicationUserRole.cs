using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Voleska.Areas.Identity.Data
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public string ApplicationUserID { get; set; }
        public string ApplicationRoleID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ApplicationRole ApplicationRole { get; set; }
    }
}
