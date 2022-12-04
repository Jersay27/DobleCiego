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
    public class EscritorController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        // GET: Escritor
        public ActionResult Index()
        {
            var main = db.T_Titulos.Where(x => x.Status != "Finalizado").ToList();
            return View(main);
        }
        public ActionResult download(int id)
        {
            var main = db.T_Titulos.Where(x => x.IdTitulo == id).Select(x => x.DocRef).FirstOrDefault();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(main, contentType, Path.GetFileName(main));
        }
        public ActionResult addEscrito(int? id)
        {
            if (id != null)
            {
                var main = db.T_Titulos.Where(x => x.IdTitulo == id).FirstOrDefault();
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
                var main = new T_Tracking();
                DateTime actualdate = DateTime.Now;
                main.idUsuario_Escritor = idUsuario;
                main.idTitulo = id;
                main.docRef_Escritor = pathDoc;
                main.escritorCreateDate = actualdate;
                db.T_Tracking.Add(main);
                db.SaveChanges();
                var status = db.T_Titulos.Where(x => x.IdTitulo == id).FirstOrDefault();
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
            var validate = db.T_Tracking.Where(x => x.docRef_Escritor == refe).ToList();
            int MyNumber = 0;
            string finalpth = refe;
            if (validate.Count > 0)
            {
                var validation = db.T_Tracking.Select(x => x.docRef_Escritor).ToList();
                while (!validation.Contains(refe))
                    MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();

            }
            else
            {
                var validation = db.T_Tracking.Select(x => x.docRef_Escritor).ToList();
                MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();
            }

            return (finalpth);
        }

    }
}