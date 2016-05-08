using System.Web.Mvc;

namespace EventCloud.Web.Controllers
{
    public class AboutController : EventCloudControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}