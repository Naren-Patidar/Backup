using System.Web;
using System.Web.Optimization;

namespace Tesco.ClubcardProducts.MCA.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles, string culture)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.unobtrusive*",
            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
            "~/Content/js/main-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/coupon").Include(
            "~/Content/js/coupon-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/personaldetails").Include(
            "~/Content/js/personalDetails-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/preferences").Include(
            "~/Content/js/preferences-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/contactpreferences").Include(
            "~/Content/js/contactPreferences-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modal").Include("~/Content/js/modal-{version}.js"));
            
            BundleTable.EnableOptimizations = true;
        }
    }
}
