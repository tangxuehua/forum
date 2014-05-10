using System.Web;
using System.Web.Optimization;

namespace Forum.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new ScriptBundle("~/content/jquery").Include(
                        "~/Content/jquery/jquery-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/content/jqueryui").Include(
                        "~/Content/jquery/jquery-ui-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/content/jqueryval").Include(
                        "~/Content/jquery/jquery.unobtrusive-ajax.min.js",
                        "~/Content/jquery/jquery.validate.min.js",
                        "~/Content/jquery/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/content/bootstrap/js").Include(
                        "~/Content/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/content/bootstrap/css").Include(
                        "~/Content/bootstrap/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}
