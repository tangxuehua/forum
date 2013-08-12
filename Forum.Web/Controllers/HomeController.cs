using System.Web.Mvc;
using Forum.Query;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private QueryService _queryService;

        public HomeController(QueryService queryService) {
            _queryService = queryService;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
