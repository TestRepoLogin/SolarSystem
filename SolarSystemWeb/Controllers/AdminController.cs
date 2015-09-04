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
        public PartialViewResult CreateObject()
        {
            ViewBag.types = new SelectList(TypesRepository.GetAll(), "Id", "Name");
            ViewBag.all = new SelectList(Repository.GetAll(), "Id", "Name");
            return PartialView("ChangeObject", new SpaceObjectDto());
        }
        
        [HttpGet]
        public PartialViewResult ChangeObject(int id)
        {
            var model = Repository.Get(id);
            ViewBag.types = new SelectList(TypesRepository.GetAll(), "Id", "Name");
            ViewBag.all = new SelectList(Repository.Get(x => x.Id != id), "Id", "Name");
            return PartialView(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeObject(SpaceObjectDto model)
        {
            if (ModelState.IsValid)
            {
                if(model.Id > 0)
                    await Repository.UpdateAsync(model);
                else
                    await Repository.AddAsync(model);
                return RedirectToAction("Index");
            }

            var typesTask = TypesRepository.GetAllAsync();
            var allTask = Repository.GetAsync(x => x.Id != model.Id);

            await Task.WhenAll(typesTask, allTask);

            ViewBag.types = new SelectList(typesTask.Result, "Id", "Name");
            ViewBag.all = new SelectList(allTask.Result, "Id", "Name");
            
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