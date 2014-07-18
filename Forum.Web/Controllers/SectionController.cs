using System.Linq;
using System.Web.Mvc;
using Forum.QueryServices;
using Forum.Web.Extensions;

namespace Forum.Web.Controllers
{
    public class SectionController : Controller
    {
        private readonly ISectionQueryService _queryService;

        public SectionController(ISectionQueryService queryService)
        {
            _queryService = queryService;
        }

        [HttpGet]
        public ActionResult GetAll(string sectionId)
        {
            var sections = _queryService.FindAll();
            return PartialView("SectionNavbarPartial", sections.Select(x => x.ToViewModel(sectionId)));
        }
    }
}