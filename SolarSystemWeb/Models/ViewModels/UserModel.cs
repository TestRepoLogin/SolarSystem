using System.Collections.Generic;
using System.Linq;
using SolarSystemWeb.Models.Identity;

namespace SolarSystemWeb.Models.ViewModels
{
    /// <summary>
    /// Модель для отображения данных о пользователе и его ролях
    /// </summary>
    public class UserModel
    {
        public UserModel(string name, IEnumerable<RoleModel> roles)
        {
            Name = name;
            Roles = roles;
        }

        public UserModel(ApplicationUser user, IEnumerable<RoleModel> roles) : this(user.UserName, roles)
        {
            Id = user.Id;
        }

        public string Id { get; set; }

        public string Name { get; set; }        

        public IEnumerable<RoleModel> Roles { get; set; }

        public string RolesString
        {
            get
            {
                return Roles.Aggregate("", (current, role) => current + (role.Name + ", ")).TrimEnd(',', ' ') ?? "";
            }
        }
    }
}