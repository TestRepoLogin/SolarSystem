﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;
using SolarSystemWeb.Models.ViewModels;

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
            var model = await Repository.GetAllAsync();
            return View(model);
        }
        
        public ActionResult TopPanel()
        {            
            var model = TypesRepository.GetAll().Where(x => !x.IsSun);
            return PartialView(model);            
        }

        public ActionResult DropDown(SpaceObjectTypeDto item)
        {
            var spaceObjects = Repository.Get(x => x.SpaceObjectTypeId == item.Id);
            var model = new DropDownViewModel(item, spaceObjects);
            return PartialView(model);
        }

        [HttpGet]
        public PartialViewResult SpaceObjectInfo(int id)
        {
            var model = Repository.Get(id);            
            return PartialView(model);
        }

        protected override void Dispose(bool disposing)
        {
            TypesRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}