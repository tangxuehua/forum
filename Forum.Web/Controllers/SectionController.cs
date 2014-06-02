using System.Threading.Tasks;
using System.Web.Mvc;
using ENode.Commanding;
using Forum.QueryServices;

namespace Forum.Web.Controllers
{
    public class SectionController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly ISectionQueryService _queryService;

        public SectionController(ICommandService commandService, ISectionQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var sections = await Task.Factory.StartNew(() => _queryService.FindAll());
            return Json(new{ success = true, data = sections }, JsonRequestBehavior.AllowGet);
        }
    }
}