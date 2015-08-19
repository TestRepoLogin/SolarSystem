using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Repositories;

namespace SolarSystemWeb.Controllers
{
    public class BaseController<TModel, TData> : Controller
                            where TModel : SimpleModel
                            where TData : class
    {
        protected readonly ICrudRepository<TModel, TData> Repository;

        public BaseController() { }

        public BaseController(ICrudRepository<TModel, TData> repository)
        {
            Repository = repository;
        }
        
        protected override void Dispose(bool disposing)
        {
            Repository.Dispose();
            base.Dispose(disposing);
        }
    }
}