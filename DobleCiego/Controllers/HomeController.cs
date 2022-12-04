using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DobleCiego.Models;

namespace DobleCiego.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        public ActionResult Index()
        {
            var main = db.T_Publicaciones.ToList();
            return View(main);
        }
        public ActionResult download(int id)
        {
            var main = db.T_Publicaciones.Where(x => x.idPublicacion == id).Select(x => x.docRef).FirstOrDefault();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(main, contentType, Path.GetFileName(main));
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}