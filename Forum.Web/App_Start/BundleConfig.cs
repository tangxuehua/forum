using System.Web.Optimization;

namespace Forum.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            //bundles.Add(new ScriptBundle("~/content/jquery/t").Include(
            //    "~/content/jquery/jquery-1.10.2.min.js"));
            //bundles.Add(new ScriptBundle("~/content/bootstrap/js/t").Include(
            //    "~/content/bootstrap/js/bootstrap.min.js"));
            //bundles.Add(new StyleBundle("~/content/bootstrap/css/t").Include(
            //    "~/content/bootstrap/css/bootstrap.min.css",
            //    "~/content/bootstrap/css/bootstrap-theme.min.css"));
            //bundles.Add(new ScriptBundle("~/content/angularjs/t").Include(
            //    "~/content/angularjs/angular.min.js"));
            //bundles.Add(new ScriptBundle("~/content/bootbox/t").Include(
            //    "~/content/bootbox/bootbox.js"));
            //bundles.Add(new ScriptBundle("~/content/js/t").Include(
            //    "~/content/js/site.js"));
            //bundles.Add(new StyleBundle("~/content/css/t").Include(
            //    "~/content/css/site.css"));



            //global
            bundles.Add(new StyleBundle("~/content/css/global").Include(
                "~/Content/libs/bootstrap/css/bootstrap.min.css",
                "~/Content/css/font-awesome.css",
                "~/Content/css/style.css",
                "~/Content/libs/toastr/toastr.min.css"));
            //"~/Content/css/animate.css"

            bundles.Add(new ScriptBundle("~/content/js/global").Include(
             "~/content/libs/jquery/jquery-1.10.2.min.js",
             "~/content/libs/bootstrap/js/bootstrap.min.js",
             "~/Content/libs/jquery/jquery.form.js",
             "~/Content/libs/toastr/toastr.min.js",
             "~/Content/libs/com.js"));

            //simditor
            bundles.Add(new ScriptBundle("~/content/js/simditor").Include(
                "~/Content/libs/simditor-2.3.6/scripts/module.js",
                "~/Content/libs/simditor-2.3.6/scripts/uploader.js",
                "~/Content/libs/simditor-2.3.6/scripts/hotkeys.js",
                "~/Content/libs/simditor-2.3.6/scripts/simditor.js"));

            bundles.Add(new StyleBundle("~/content/css/simditor").Include(
              "~/Content/libs/simditor-2.3.6/styles/simditor.css"));

            //jquery.validate
            bundles.Add(new ScriptBundle("~/content/js/validate").Include(
              "~/Content/libs/validate/jquery.validate.min.js",
              "~/Content/libs/validate/messages_zh.min.js"));
            //BundleTable.EnableOptimizations = true;
        }
    }
}
