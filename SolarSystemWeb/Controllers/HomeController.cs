using System.Web.Mvc;
using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;
using SolarSystemWeb.Models.Repositories;

namespace SolarSystemWeb.Controllers
{
    public class HomeController : BaseController<SpaceObjectDto, SpaceObject>
    {
        public HomeController(ICrudRepository<SpaceObjectDto, SpaceObject> repository) 
            : base(repository) { }

        public ActionResult Index()
        {
            using (var pr = new SpaceObjectRepository())
            {
                var model = pr.Get(x => x.SpaceObjectTypeId == 2);
                return View(model);
            }            
        }

    }
}