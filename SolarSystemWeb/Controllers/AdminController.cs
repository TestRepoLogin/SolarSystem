using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<ActionResult> Index(int? typeId)
        {            
            ViewBag.UserName = HttpContext.User.Identity.Name;

            var typesTask = TypesRepository.GetAllAsync();
            var allTask = typeId.HasValue ? Repository.GetAsync(x => x.SpaceObjectTypeId == typeId.Value): Repository.GetAllAsync();
            await Task.WhenAll(typesTask, allTask);

            var items = new List<SelectListItem> { new SelectListItem { Value = "null", Text = "Все типы", Selected = true } };
            items.AddRange(typesTask.Result.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }));
            ViewBag.types = items;
            return View(allTask.Result);
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
            ValidateImage("MainImage", mainImg);
            ValidateImage("OrbitImage", orbitImg);

            if (ModelState.IsValid)
            {
                if (mainImg != null)
                    model.MainImage = GetImageArray(mainImg);

                if (orbitImg != null)
                    model.OrbitImage = GetImageArray(orbitImg);

                if (model.Id > 0)
                {
                    var excluding = new List<Expression<Func<SpaceObject, byte[]>>>();

                    if(model.MainImage == null)                    
                        excluding.Add(x => x.MainImage);

                    if (model.OrbitImage == null)
                        excluding.Add(x => x.OrbitImage);

                    await Repository.UpdateAsync(model, excluding);
                }
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

        [NonAction]
        private byte[] GetImageArray(HttpPostedFileBase file)
        {
            var res = new byte[file.InputStream.Length];
            file.InputStream.Read(res, 0, (int)file.InputStream.Length);
            return res;
        }

        [NonAction]
        private void ValidateImage(string key, HttpPostedFileBase file)
        {
            if (file != null && !ApplicationSettings.Instance.AllowedTypes.Contains(file.ContentType))
            {
                ModelState.AddModelError(key, "Недопустимый тип загружаемого файла");
                return;
            }

            if (file != null && file.ContentLength > ApplicationSettings.Instance.MaxImageSize)
            {
                ModelState.AddModelError(key, String.Format("Размер загружаемого изображения не должен превышать {0} байт", ApplicationSettings.Instance.MaxImageSize));
            }
        }

        protected override void Dispose(bool disposing)
        {
            TypesRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}