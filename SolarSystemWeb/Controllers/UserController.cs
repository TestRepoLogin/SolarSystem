using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SolarSystemWeb.Models.Exceptions;
using SolarSystemWeb.Models.Identity;
using SolarSystemWeb.Models.ViewModels;

namespace SolarSystemWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        private ApplicationRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        public ActionResult Index()
        {
            ViewBag.UserName = HttpContext.User.Identity.Name;
            var users = UserManager.Users.ToList();
            var model = users.Select(user => new UserModel(user, GetRolesByUser(user)));
            return View(model);
        }

        [NonAction]
        private IEnumerable<RoleModel> GetRolesByUser(ApplicationUser user)
        {
            try
            {
                var names = UserManager.GetRoles(user.Id);
                var roles = names.Select(id => RoleManager.Roles.FirstOrDefault(role => role.Name == id)).ToList();
                return roles.Select(x => (RoleModel)x);
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new PermissionException($"При попытке получения ролей пользователя {user.UserName} возникла ошибка.", ex));
                return null;
            }
            
        }

        /// <summary>
        /// Добавление.удаление правд администратора пользователю        
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public async Task<JsonResult> ChangeAdminPermission(string userId, bool flag)
        {
            var result = flag ? await UserManager.AddToRoleAsync(userId, "Admin") : await UserManager.RemoveFromRoleAsync(userId, "Admin");

            if (!result.Succeeded)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new PermissionException(result.Errors.Aggregate("", (current, error) => current + (error + ", ")).TrimEnd(',', ' ')));
                Response.StatusCode = 500;
            }

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = "" };
        }

        public async Task<JsonResult> Delete(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);           
            var result = await UserManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new PermissionException(result.Errors.Aggregate("", (current, error) => current + (error + ", ")).TrimEnd(',', ' ')));                
                Response.StatusCode = 500;
            }
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = "" };
        }        
    }
}