using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Controllers
{
    public class HomeController : BaseController<SpaceObjectDto, SpaceObject>
    {
        public HomeController(ICrudRepository<SpaceObjectDto, SpaceObject> repository,
                              ICrudRepository<SpaceObjectTypeDto, SpaceObjectType> typesRepository)
            : base(repository)
        {
            TypesRepository = typesRepository;
        }

        protected readonly ICrudRepository<SpaceObjectTypeDto, SpaceObjectType> TypesRepository;

        public async Task<ActionResult> Index()
        {
            var model = await Repository.GetAsync(x => x.SpaceObjectTypeId == 2);
            return View(model);
        }
        
        public ActionResult SpaceObjectTypes()
        {            
            var model = TypesRepository.GetAll().Where(x => !x.IsSun);
            return PartialView(model);            
        }

        protected override void Dispose(bool disposing)
        {
            TypesRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}