using System;
using System.Web.Mvc;

namespace SolarSystemWeb.Controllers
{
    public class ErrorController : Controller
    {
        
        public ActionResult NotFound(string aspxerrorpath)
        {
            return View((object)aspxerrorpath);
        }

        public ActionResult InternalServerError()
        {
            return View();
        }                       
    }
}