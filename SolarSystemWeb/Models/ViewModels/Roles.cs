using System.ComponentModel.DataAnnotations;
using SolarSystemWeb.Models.Identity;

namespace SolarSystemWeb.Models.ViewModels
{
    public class RoleModel
    {        
        public string Id { get; set; }

        [Required(ErrorMessage = "Нужно указать название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Нужно указать описание")]
        public string Description { get; set; }

        public static explicit operator RoleModel(ApplicationRole role)
        {
            return role == null ? null : new RoleModel {Id = role.Id, Name = role.Name, Description = role.Description};
        }
    }
}