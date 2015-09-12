using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SolarSystemWeb.Models.Identity;

namespace SolarSystemWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private ApplicationRoleManager RoleManager => HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        public ActionResult Create()
        {
            return View("ChangeRole");
        }

        [HttpPost]
        public async Task<ActionResult> ChangeRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = IdentityResult.Failed();
                if (string.IsNullOrEmpty(model.Id))
                {
                    var role = new ApplicationRole {Id = Guid.NewGuid().ToString(), Name = model.Name, Description = model.Description};
                    result = await RoleManager.CreateAsync(role);
                }
                else
                {
                    ApplicationRole role = await RoleManager.FindByIdAsync(model.Id);
                    if (role != null)
                    {
                        role.Description = model.Description;
                        role.Name = model.Name;
                        result = await RoleManager.UpdateAsync(role);
                    }
                }

                if (result.Succeeded)
                    return RedirectToAction("Index");

                ModelState.AddModelError("", "Что-то пошло не так");
            }
            return View("ChangeRole", model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
                return View("ChangeRole", new RoleModel { Name = role.Name, Description = role.Description });

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await RoleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Description = model.Description;
                    role.Name = model.Name;
                    IdentityResult result = await RoleManager.UpdateAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("Index");

                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            return View("ChangeRole", model);
        }

        public async Task<JsonResult> Delete(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
            }
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = "OK" };
        }        
    }
}