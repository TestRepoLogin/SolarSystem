using System.ComponentModel.DataAnnotations;

namespace SolarSystemWeb.Models.Identity
{
    public class RoleModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Нужно указать название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Нужно указать описание")]
        public string Description { get; set; }
    }
}