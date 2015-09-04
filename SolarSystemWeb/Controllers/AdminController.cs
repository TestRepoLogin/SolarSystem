using System.Threading.Tasks;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Controllers
{
    public class AdminController : BaseController<SpaceObjectDto, SpaceObject>
    {
        public AdminController(ICrudRepository<SpaceObjectDto, SpaceObject> repository,
                              ICrudRepository<SpaceObjectTypeDto, SpaceObjectType> typesRepository)
            : base(repository)
        {
            TypesRepository = typesRepository;
        }

        protected readonly ICrudRepository<SpaceObjectTypeDto, SpaceObjectType> TypesRepository;

        public async Task<ActionResult> Index()
        {
            var model = await Repository.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        public JsonResult DeleteObject(int id)
        {
            Repository.Delete(id);
            return new JsonResult {JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = "OK" };
        }

        protected override void Dispose(bool disposing)
        {
            TypesRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}