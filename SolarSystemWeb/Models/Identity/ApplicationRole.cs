using Microsoft.AspNet.Identity.EntityFramework;

namespace SolarSystemWeb.Models.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public virtual string Description { get; set; }
    }
}