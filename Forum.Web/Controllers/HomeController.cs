using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Forum.Web.Models;
using ENode.Commanding;
using Forum.QueryServices;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IPostQueryService _postQueryService;

        public HomeController(ICommandService commandService, IPostQueryService postQueryService)
        {
            _commandService = commandService;
            _postQueryService = postQueryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
