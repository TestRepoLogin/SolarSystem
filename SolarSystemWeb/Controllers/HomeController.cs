using System.Web.Mvc;
using SolarSystemWeb.Models.Repositories;

namespace SolarSystemWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            using (var pr = new SpaceObjectRepository())
            {
                var model = pr.Get(x => x.Id >2);
                //var model = pr.Get(x => x.SpaceObjectTypeId == 2);
                return View(model);
            }            
        }
    }
}