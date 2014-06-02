using System.Web.Mvc;
using Forum.Web.Attributes;

namespace Forum.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExceptionAttribute());
        }
    }
}
