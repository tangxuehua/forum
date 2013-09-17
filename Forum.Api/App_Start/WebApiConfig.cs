using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Forum.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "SectionPostsApi",
                routeTemplate: "api/{controller}/sections/{sectionId}/pages/{pageIndex}",
                defaults: null,
                constraints: new { sectionId = new GuidConstraint(), pageIndex = @"\d+" }
            );
            config.Routes.MapHttpRoute(
                name: "PostsApi",
                routeTemplate: "api/{controller}/pages/{pageIndex}",
                defaults: null,
                constraints: new { pageIndex = @"\d+" }
            );
        }
    }

    public class GuidConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (values.ContainsKey(parameterName))
            {
                string stringValue = values[parameterName] as string;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    Guid guidValue;
                    return Guid.TryParse(stringValue, out guidValue) && (guidValue != Guid.Empty);
                }
            }

            return false;
        }
    }
}
