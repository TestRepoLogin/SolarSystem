using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SolarSystemWeb.Models.Identity
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("IdentityDb") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        //public virtual IDbSet<TUser> Users { get; set; }
    }
}