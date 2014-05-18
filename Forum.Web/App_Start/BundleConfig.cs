using System.Web.Optimization;

namespace Forum.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new ScriptBundle("~/content/jquery").Include(
                "~/Content/jquery/jquery-1.10.2.min.js"));
            bundles.Add(new ScriptBundle("~/content/bootstrap/js").Include(
                "~/Content/bootstrap/js/bootstrap.min.js"));
            bundles.Add(new StyleBundle("~/content/bootstrap/css").Include(
                "~/Content/bootstrap/css/bootstrap.min.css",
                "~/Content/bootstrap/css/bootstrap-theme.min.css"));
            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/site.css"));
        }
    }
}
