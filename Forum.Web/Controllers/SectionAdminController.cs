using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ECommon.IO;
using ECommon.Utilities;
using ENode.Commanding;
using Forum.Commands.Sections;
using Forum.QueryServices;
using Forum.Web.Extensions;
using Forum.Web.Models;
using Forum.Web.Services;

namespace Forum.Web.Controllers
{
    public class SectionAdminController : Controller
    {
        private readonly ICommandService _commandService;
        private readonly ISectionQueryService _queryService;
        private readonly IContextService _contextService;

        public SectionAdminController(ICommandService commandService, ISectionQueryService queryService, IContextService contextService)
        {
            _commandService = commandService;
            _queryService = queryService;
            _contextService = contextService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            if (_contextService.CurrentAccount.AccountName != "admin")
            {
                return View("NoPermission");
            }
            var sections = _queryService.FindAll();
            return View(sections.Select(x => x.ToViewModel(null)));
        }
        [HttpGet]
        public ActionResult Find(string id, string option)
        {
            return Json(new
            {
                success = true,
                data = _queryService.FindDynamic(id, option)
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AjaxAuthorize]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        public async Task<ActionResult> Create(CreateSectionModel model)
        {
            if (_contextService.CurrentAccount.AccountName != "admin")
            {
                return Json(new { success = false, errorMsg = "只有系统管理员才能新建版块。" });
            }

            var result = await _commandService.SendAsync(new CreateSectionCommand(ObjectId.GenerateNewStringId(), model.Name, model.Description));

            if (result.Status != AsyncTaskStatus.Success)
            {
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            return Json(new { success = true });
        }
        [HttpPost]
        [AjaxAuthorize]
        [AjaxValidateAntiForgeryToken]
        [AsyncTimeout(5000)]
        public async Task<ActionResult> Update(EditSectionModel model)
        {
            if (_contextService.CurrentAccount.AccountName != "admin")
            {
                return Json(new { success = false, errorMsg = "只有系统管理员才能修改版块。" });
            }

            var result = await _commandService.SendAsync(new ChangeSectionNameCommand(model.Id, model.Name, model.Description));

            if (result.Status != AsyncTaskStatus.Success)
            {
                return Json(new { success = false, errorMsg = result.ErrorMessage });
            }

            return Json(new { success = true });
        }
    }
}