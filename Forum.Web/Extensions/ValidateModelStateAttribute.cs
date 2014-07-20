using System;
using System.Linq;
using System.Web.Mvc;

namespace Forum.Web.Extensions
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                if (modelState.Count > 0 && modelState.Count(x => x.Value.Errors.Count > 0) > 0)
                {
                    var errorMessage = modelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage;
                    throw new ModelStateException(errorMessage);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class ModelStateException : Exception
    {
        public ModelStateException(string message) : base(message) { }
    }
}