using System.Web;
using System.Web.Optimization;

namespace DobleCiego
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/modernizr-*",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/toastr.min.js",
                        "~/Content/datatable/datatables.min.js",
                        "~/Content/datatable/Buttons-2.3.3/js/dataTables.buttons.min.js",
                        "~/Content/datatable/Buttons-2.3.3/js/buttons.flash.min.js",
                        "~/Content/datatable/JSZip-2.5.0/jszip.min.js",
                        "~/Content/datatable/pdfmake-0.1.36/pdfmake.min.js",
                        "~/Content/datatable/pdfmake-0.1.36/vfs_fonts.js",
                        "~/Content/datatable/Buttons-2.3.3/js/buttons.html5.min.js",
                        "~/Content/datatable/Buttons-2.3.3/js/buttons.print.min.js"
                         ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/datatable/Buttons-2.3.3/css/datatables.min.css",
                      "~/Content/datatable/buttons.dataTables.min.css",
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/toastr.min.css",
                      "~/css/all.css",
                      "~/Content/site.css"));
        }
    }
}
