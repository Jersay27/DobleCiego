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
    public class RevisorController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        // GET: Revisor
        public ActionResult Index()
        {
            var main = (from tracking in db.T_Tracking where tracking.idUsuario_Revisor == null
                        join revisor in db.T_Usuarios on tracking.idUsuario_Revisor equals revisor.IdUsuario into revisorlist
                        from revisor in revisorlist.DefaultIfEmpty()
                        join escritor in db.T_Usuarios on tracking.idUsuario_Escritor equals escritor.IdUsuario into escritorlst
                        from escritor in escritorlst.DefaultIfEmpty()
                        join titulo in db.T_Titulos on tracking.idTitulo equals titulo.IdTitulo into titulolst
                        from titulo in titulolst.DefaultIfEmpty()
                        select new modelTracking
                        {
                            idTracking = tracking.IdTrack,
                            titulo = titulo.Titulo,
                            imgRef = titulo.ImagenRef,
                            revisorName = revisor.Nombre + " " + revisor.Apellido,
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
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(main, contentType, Path.GetFileName(main));
        }
        public ActionResult revisar(int? id) 
        {
            if (id != null)
            {
                var main = (from tracking in db.T_Tracking
                            where tracking.IdTrack == id
                            join revisor in db.T_Usuarios on tracking.idUsuario_Revisor equals revisor.IdUsuario into revisorlist
                            from revisor in revisorlist.DefaultIfEmpty()
                            join escritor in db.T_Usuarios on tracking.idUsuario_Escritor equals escritor.IdUsuario into escritorlst
                            from escritor in escritorlst.DefaultIfEmpty()
                            join titulo in db.T_Titulos on tracking.idTitulo equals titulo.IdTitulo into titulolst
                            from titulo in titulolst.DefaultIfEmpty()
                            select new modelTracking
                            {
                                idTracking = tracking.IdTrack,
                                titulo = titulo.Titulo,
                                imgRef = titulo.ImagenRef,
                                revisorName = revisor.Nombre + " " + revisor.Apellido,
                                escritorName = escritor.Nombre + " " + escritor.Apellido,
                                docRefEscritor = tracking.docRef_Escritor,
                                docRefRevisor = tracking.docRef_Revisor,
                                revisorDate = tracking.revisorCreateDate,
                                escritorDate = tracking.escritorCreateDate,
                                descripcion = titulo.Propuesta
                            }).FirstOrDefault();
                return View(main);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UploadFile(int id, HttpPostedFileBase doc)
        {
            try
            {
                string pathDoc = "";
                if (doc.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(doc.FileName);
                    _FileName.Split('.');
                    string _path = Path.Combine(Server.MapPath("~/Documents"), _FileName);
                    string filename = fnalpathDoc(_path);
                    string formato = _FileName.Split('.').Last();
                    string _path2 = Path.Combine(Server.MapPath("~/Documents"), filename + "." + formato);
                    pathDoc = @"\Documents\" + filename + "." + formato;
                    doc.SaveAs(_path2);
                }
                int idUsuario = Convert.ToInt32(User.Identity.Name);
                var main = db.T_Tracking.Where(x => x.IdTrack == id).FirstOrDefault();
                DateTime actualdate = DateTime.Now;
                main.idUsuario_Revisor = idUsuario;
                main.docRef_Revisor = pathDoc;
                main.revisorCreateDate = actualdate;
                db.SaveChanges();
                
                var status = db.T_Titulos.Where(x => x.IdTitulo == main.idTitulo).FirstOrDefault();
                status.Status = "En Proceso";
                db.SaveChanges();
                TempData["Message"] = "Se aggrego un nuevo titulo";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Error al guardar archivos";
                return RedirectToAction("Index");
            }

        }
        public Random a = new Random();

        private string fnalpathDoc(string refe)
        {
            var validate = db.T_Tracking.Where(x => x.docRef_Revisor == refe).ToList();
            int MyNumber = 0;
            string finalpth = refe;
            if (validate.Count > 0)
            {
                var validation = db.T_Tracking.Select(x => x.docRef_Revisor).ToList();
                while (!validation.Contains(refe))
                    MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();

            }
            else
            {
                var validation = db.T_Tracking.Select(x => x.docRef_Revisor).ToList();
                MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();
            }

            return (finalpth);
        }
    }
}