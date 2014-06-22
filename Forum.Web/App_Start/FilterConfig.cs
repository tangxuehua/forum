using System.Web.Mvc;
using Forum.Web.Extensions;

namespace Forum.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionAttribute());
            filters.Add(new JsonHandlerAttribute());
        }
    }
}
