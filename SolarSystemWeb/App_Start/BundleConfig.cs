using System.Web.Optimization;

namespace SolarSystemWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/starry").Include("~/Scripts/starry.js"));
            bundles.Add(new ScriptBundle("~/bundles/mainPage").Include("~/Scripts/main_page.js"));

            bundles.Add(new StyleBundle("~/Styles/main").Include("~/Styles/main.css"));
            bundles.Add(new StyleBundle("~/Styles/starry").Include("~/Styles/starry.css"));
        }
    }
}