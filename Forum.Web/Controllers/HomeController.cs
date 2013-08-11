using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENode.Domain;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private Forum.QueryService.QueryService _queryService;
        private IMemoryCache _memoryCache;

        public HomeController(Forum.QueryService.QueryService queryService, IMemoryCache memoryCache) {
            _queryService = queryService;
            _memoryCache = memoryCache;
        }

        public ActionResult Index()
        {
            ViewBag.Message = "修改此模板以快速启动你的 ASP.NET MVC 应用程序。";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "你的应用程序说明页。";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "你的联系方式页。";

            return View();
        }
    }
}
