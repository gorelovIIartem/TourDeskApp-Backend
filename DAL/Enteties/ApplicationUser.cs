using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual User User { get; set; }

    }
}
