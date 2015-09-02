using System.Web.Optimization;

namespace SolarSystemWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/libs/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/libs/bootstrap.js", 
                                                                        "~/Scripts/libs/bootstrap-multiselect.js"));     
                   
            bundles.Add(new ScriptBundle("~/bundles/starry").Include("~/Scripts/starry.js"));
            bundles.Add(new ScriptBundle("~/bundles/mainPage").Include("~/Scripts/main_page.js"));
            bundles.Add(new ScriptBundle("~/bundles/info").Include("~/Scripts/info.js"));
            bundles.Add(new ScriptBundle("~/bundles/solar").Include("~/Scripts/solar/classes.js", 
                                                                    "~/Scripts/solar/system.js"));

            bundles.Add(new StyleBundle("~/Styles/bootstrap").Include("~/Content/bootstrap.css", 
                                                                      "~/Content/bootswatch/superhero/bootstrap.css", 
                                                                      "~/Content/bootstrap-multiselect"));
            bundles.Add(new StyleBundle("~/Styles/main").Include("~/Content/main.css"));
            bundles.Add(new StyleBundle("~/Styles/starry").Include("~/Content/starry.css"));
            bundles.Add(new StyleBundle("~/Styles/index").Include("~/Content/index.css"));
            bundles.Add(new StyleBundle("~/Styles/info").Include("~/Content/info.css"));            
            bundles.Add(new StyleBundle("~/Styles/error404").Include("~/Content/error404.css"));            
        }
    }
}