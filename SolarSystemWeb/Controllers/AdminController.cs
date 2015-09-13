using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Application;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Controllers
{
    [Authorize(Roles = "Admin")]
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
            ViewBag.UserName = HttpContext.User.Identity.Name;
            var model = await Repository.GetAllAsync();
            return View(model);
        }

        public async Task<ActionResult> ObjectTypes()
        {
            var model = await TypesRepository.GetAllAsync();
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
        public PartialViewResult CreateObjectType()     
        {
            return PartialView("ChangeObjectType", new SpaceObjectTypeDto());
        }

        [HttpGet]
        public ActionResult ChangeObject(int id)
        {           
            var model = Repository.Get(id);
            ViewBag.types = new SelectList(TypesRepository.GetAll(), "Id", "Name");
            ViewBag.all = new SelectList(Repository.Get(x => x.Id != id), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeObject(SpaceObjectDto model, HttpPostedFileBase  mainImg, HttpPostedFileBase orbitImg)
        {
            if (mainImg != null && mainImg.ContentLength > ApplicationSettings.Instance.MaxImageSize)
            {
                ModelState.AddModelError("MainImage", $"Размер загружаемого изображения не должен превышать {ApplicationSettings.Instance.MaxImageSize} байт");
            }

            if (orbitImg != null && orbitImg.ContentLength > ApplicationSettings.Instance.MaxImageSize)
            {
                ModelState.AddModelError("OrbitImage", $"Размер загружаемого изображения не должен превышать {ApplicationSettings.Instance.MaxImageSize} байт");
            }

            if (ModelState.IsValid)
            {
                if (mainImg != null)
                {                                         
                    model.MainImage = new byte[mainImg.InputStream.Length];
                    mainImg.InputStream.Read(model.MainImage, 0, (int)mainImg.InputStream.Length);
                }

                if (orbitImg != null)
                {
                    model.OrbitImage = new byte[orbitImg.InputStream.Length];
                    orbitImg.InputStream.Read(model.OrbitImage, 0, (int)orbitImg.InputStream.Length);
                }

                if (model.Id > 0)
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
        public ActionResult ChangeObjectType(int id)
        {
            var model = TypesRepository.Get(id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeObjectType(SpaceObjectTypeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                    await TypesRepository.UpdateAsync(model);
                else
                    await TypesRepository.AddAsync(model);
                return RedirectToAction("ObjectTypes");
            }

            return View(new SpaceObjectTypeDto());
        }

        [HttpGet]
        public JsonResult DeleteObject(int id)
        {
            Repository.Delete(id);
            return new JsonResult {JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = "OK" };
        }

        [HttpGet]
        public JsonResult DeleteObjectType(int id)
        {
            TypesRepository.Delete(id);
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = "OK" };
        }
        
        protected override void Dispose(bool disposing)
        {
            TypesRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}