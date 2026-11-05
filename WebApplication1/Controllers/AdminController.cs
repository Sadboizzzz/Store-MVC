using System.Web.Mvc;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
