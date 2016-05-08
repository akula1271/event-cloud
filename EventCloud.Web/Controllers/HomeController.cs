using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace EventCloud.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : EventCloudControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}