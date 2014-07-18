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
        public ActionResult Index(string sectionId)
        {
            var sections = _queryService.FindAll();
            return PartialView("SectionNavbarPartial", sections.Select(x => x.ToViewModel(sectionId)));
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            return Json(new
            {
                success = true,
                data = _queryService.FindAll()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}