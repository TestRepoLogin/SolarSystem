using System.Web.Mvc;
using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Controllers
{
    public class ImageController : BaseController<SpaceObjectDto, SpaceObject>
    {
        public ImageController(ICrudRepository<SpaceObjectDto, SpaceObject> repository)
            : base(repository)
        {
            
        }

        public ActionResult Show(int id)
        {
            var imageData = Repository.Get(id).MainImage;
            return File(imageData, "image/jpg");
        }
    }
}