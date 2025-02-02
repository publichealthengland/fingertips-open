using System.Web.Optimization;

namespace Fpm.MainUI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var cssPath = "~/" + AppConfig.CssPath;
            var jsPath = "~/" + AppConfig.JsPath;
            var angularDist = "~/" + AppConfig.AngularAppDistPath;

            bundles.Add(new ScriptBundle("~/angular-dist")
                .Include(angularDist + "main.js")
                .Include(angularDist + "polyfills.js")
                .Include(angularDist + "runtime.js")
                .Include(angularDist + "scripts.js")
                .Include(angularDist + "styles.js")
                .Include(angularDist + "vendor.js")
                );

            bundles.Add(new StyleBundle("~/css/site")
               .Include(cssPath + "bootstrap.css")
               .Include(cssPath + "select2.min.css")
               .Include(cssPath + "fingertips.css")
               .Include(cssPath + "dashboard.css")
               .Include(cssPath + "profile-manager.css")
               );

            bundles.Add(new ScriptBundle("~/js/jquery")
                .Include(jsPath + "jquery-3.3.1.min.js")
                .Include(jsPath + "jquery-ui-1.12.1.min.js")
                .Include(jsPath + "jquery-migrate-1.3.0.js")
                .Include(jsPath + "jquery.validate.min.js")
                .Include(jsPath + "jquery.validate.unobtrusive.min.js")
                );

            bundles.Add(new ScriptBundle("~/js/tablesorter")
                .Include(jsPath + "jquery.tablesorter.js")
                .Include(jsPath + "jquery.tablesorter.widgets.js")
                );

            bundles.Add(new ScriptBundle("~/js/underscore")
               .Include(jsPath + "underscore-min.js"));

            bundles.Add(new ScriptBundle("~/js/modernizr")
              .Include(jsPath + "modernizr-2.6.2.min.js"));

            bundles.Add(new ScriptBundle("~/js/bootstrap")
               .Include(jsPath + "bootstrap.js"));

            bundles.Add(new ScriptBundle("~/js/select")
                .Include(jsPath + "select2.min.js"));

            bundles.Add(new ScriptBundle("~/js/common")
               .Include(jsPath + "included-on-all-pages.js"));

            bundles.Add(new ScriptBundle("~/profile.js")
                .Include(jsPath + "Profile/profile.js"));

            bundles.Add(new ScriptBundle("~/content.js")
                .Include(jsPath + "Content/content.js"));

            bundles.Add(new ScriptBundle("~/areas.js")
                .Include(jsPath + "Areas/areas.js"));

            bundles.Add(new ScriptBundle("~/user.js")
                .Include(jsPath + "User/user.js"));

            bundles.Add(new ScriptBundle("~/documents.js")
                .Include(jsPath + "Documents/documents.js"));

            bundles.Add(new ScriptBundle("~/exception-log.js")
                .Include(jsPath + "ExceptionLog/exception-log.js"));

            bundles.Add(new ScriptBundle("~/userfeedback.js")
                .Include(jsPath + "UserFeedback/userfeedback.js"));

            bundles.Add(new ScriptBundle("~/PageCoreData.js")
                .Include(jsPath + "PageCoreData.js"));

            bundles.Add(new ScriptBundle("~/PageProfilesAndIndicators.js")
                .Include(jsPath + "PageProfilesAndIndicators.js"));
        }
    }
}
