using System.Web;
using System.Web.Optimization;

namespace LaTuerca
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryCompra").Include(
                      "~/Scripts/NuevaCompra.js",
                      "~/Scripts/NuevaCompraScript.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryVenta").Include(
                      "~/Scripts/NuevaVenta.js",
                      "~/Scripts/NuevaVentaScript.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/app.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/Moment.js",
                      "~/Scripts/moment-with-locales.js",
                      "~/Scripts/bootstrap-datetimepicker.min.js",

                      "~/Scripts/input-mask/jquery.inputmask.js",
                      "~/Scripts/input-mask/jquery.inputmask.extensions.js",
                      "~/Scripts/input-mask/jquery.inputmask.date.extensions.js",
                      "~/Scripts/input-mask/jquery.inputmask.numeric.extensions.js",
                      "~/Scripts/input-mask/jquery.inputmask.phone.extensions.js",
                      "~/Scripts/input-mask/jquery.inputmask.regex.extensions.js",

                      "~/Scripts/datatablesbuttons/jquery.dataTables.min.js",
                      "~/Scripts/datatablesbuttons/dataTables.bootstrap4.min.js",
                      "~/Scripts/datatablesbuttons/dataTables.buttons.min.js",
                      "~/Scripts/datatablesbuttons/buttons.flash.min.js",
                      "~/Scripts/datatablesbuttons/jszip.min.js",
                      "~/Scripts/datatablesbuttons/pdfmake.min.js",
                      "~/Scripts/datatablesbuttons/vfs_fonts.js",
                      "~/Scripts/datatablesbuttons/buttons.html5.min.js",
                      "~/Scripts/datatablesbuttons/buttons.print.min.js",
                      "~/Scripts/datatablesbuttons/buttons.bootstrap.min.js",
                      "~/Scripts/datatablesbuttons/buttons.colVis.min.js",
                      "~/Scripts/datatablesbuttons/dataTables.buttons.min.js",
                      "~/Scripts/datatablesbuttons/jquery-1.12.3.js",

                      "~/Content/iCheck/icheck.js",
                      "~/Scripts/jquery.easy-autocomplete.min.js",
                      "~/Scripts/jquery-ui.min.js",
                      "~/Scripts/selectize.js",
                      "~/Scripts/fileinput.min.js",
                      "~/Scripts/jquery.number.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/dataTables.bootstrap4.min.css",
                      "~/Content/AdminLTE.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/buttons.dataTables.min.css",
                      "~/Content/skins/_all-skins.min.css",
                      "~/Content/easy-autocomplete.min.css",
                      "~/Content/easy-autocomplete.themes.min.css",
                      "~/Content/datepicker.css",
                      "~/Content/datepicker3.css",
                      "~/Content/skins/skin-blue.min.css",
                      "~/Content/icons.css",
                      "~/Content/iCheck/all.css",
                      "~/Content/selectize.bootstrap2.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/jquery-ui-1.10.3.custom.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui.theme.css",
                      "~/Content/print.css",
                      "~/Content/bootstrap-fileinput/css/fileinput.css",
                      "~/Content/bootstrap-fileinput/bootstrap-fileinput/themes/fa/theme.js",
                      "~/Content/bootstrap-fileinput/bootstrap-fileinput/themes/gly/theme.js",
                      "~/Content/font-awesome.min.css"));
        }
    }
}