using System.Web.Mvc;

namespace Forum.Web.Controllers
{
    public class PostController : Controller
    {
        public ActionResult Index(string sectionId, string authorId, int? pageIndex)
        {
            ViewBag.SectionId = sectionId;
            ViewBag.AuthorId = authorId;
            ViewBag.PageIndex = pageIndex == null ? 1 : pageIndex.Value;
            return View();
        }
        public ActionResult Detail(string id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}