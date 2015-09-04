using Microsoft.AspNet.Identity.EntityFramework;

namespace SolarSystemWeb.Models.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}