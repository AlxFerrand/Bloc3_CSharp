using Bloc3_CSharp.CustomTagHelpers;
using Microsoft.AspNetCore.Identity;

namespace Bloc3_CSharp.Models
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<IdentityUser> Members { get; set; }
        public IEnumerable<IdentityUser> NonMembers { get; set; }
    }
}
