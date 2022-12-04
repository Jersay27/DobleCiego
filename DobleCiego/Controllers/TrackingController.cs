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
    public class TrackingController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        // GET: Tracking
        public ActionResult Index()
        {
            var main = (from tracking in db.T_Tracking
                        join revisor in db.T_Usuarios on tracking.idUsuario_Revisor equals revisor.IdUsuario into revisorlist from revisor in revisorlist.DefaultIfEmpty()
                        join escritor in db.T_Usuarios on tracking.idUsuario_Escritor equals escritor.IdUsuario into escritorlst from escritor in escritorlst.DefaultIfEmpty()
                        join titulo in db.T_Titulos on tracking.idTitulo equals titulo.IdTitulo into titulolst from titulo in titulolst.DefaultIfEmpty()
                        select new modelTracking{
                        idTracking = tracking.IdTrack,
                        titulo = titulo.Titulo,
                        revisorName = revisor.Nombre +" " + revisor.Apellido,
                        escritorName = escritor.Nombre + " " + escritor.Apellido,
                        docRefEscritor = tracking.docRef_Escritor,
                        docRefRevisor = tracking.docRef_Revisor,
                        revisorDate = tracking.revisorCreateDate,
                        escritorDate = tracking.escritorCreateDate
                        }).ToList(); 
            return View(main);
        }
        public ActionResult downloadEscritor(int id)
        {
            var main = db.T_Tracking.Where(x => x.IdTrack == id).Select(x => x.docRef_Escritor).FirstOrDefault();
            string file = @"c:\someFolder\foo.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(main, contentType, Path.GetFileName(main));
        }
        public ActionResult downloadRevisor(int id)
        {
            var main = db.T_Tracking.Where(x => x.IdTrack == id).Select(x => x.docRef_Revisor).FirstOrDefault();
            string file = @"c:\someFolder\foo.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(main, contentType, Path.GetFileName(main));
        }
    }
}