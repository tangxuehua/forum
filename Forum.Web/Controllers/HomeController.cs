using Microsoft.AspNetCore.Mvc;
using ENode.Commanding;
using Forum.QueryServices;
using Forum.Web.Services;
using ECommon.Logging;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly IPostQueryService _postQueryService;
        private readonly IContextService _contextService;
        private readonly ILogger _logger;

        public HomeController(ICommandService commandService, IPostQueryService postQueryService, IContextService contextService, ILoggerFactory loggerFactory)
        {
            _commandService = commandService;
            _postQueryService = postQueryService;
            _contextService = contextService;
            _logger = loggerFactory.Create(GetType());
        }

        public IActionResult Index()
        {
            var currentUser = _contextService.GetCurrentAccount(HttpContext);
            if (currentUser != null)
            {
                _logger.InfoFormat("Current login account, accountId: {0}, accountName: {1}", currentUser.AccountId, currentUser.AccountName);
            }
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
    }
}
