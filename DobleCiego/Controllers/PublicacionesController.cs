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
    public class PublicacionesController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        // GET: Publicaciones
        public ActionResult Index()
        {
            var main = db.T_Publicaciones.ToList();
            return View(main);
        }
        public ActionResult crearPublicacion()
        {
            return View();
        }
        public ActionResult UploadFile(string textArea, string firstName, HttpPostedFileBase img, HttpPostedFileBase doc)
        {
            try
            {
                string pathImg = "";
                string pathDoc = "";

                if (img.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(img.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Imagenes"), _FileName);
                    string filename = fnalpath(_path);
                    string formato = _FileName.Split('.').Last();
                    string _path2 = Path.Combine(Server.MapPath("~/Imagenes"), filename + "." + formato);
                    pathImg = @"\Imagenes\" + filename + "." + formato;
                    img.SaveAs(_path2);
                }
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
                var main = new T_Publicaciones();
                DateTime actualdate = DateTime.Now;
                main.idUsuario_Creador = idUsuario;
                main.Descripcion = textArea;
                main.Titulo = firstName;
                main.docRef = pathDoc;
                main.imgRef = pathImg;
                main.CreationDate = actualdate;
                db.T_Publicaciones.Add(main);
                db.SaveChanges();
                TempData["Message"] = "Se aggrego una nueva publicacion";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Error al guardar archivos";
                return RedirectToAction("Index");
            }

        }
        public ActionResult modificarPublicacion(int? id) 
        {
            if (id != null)
            {
                var main = db.T_Publicaciones.Where(x => x.idPublicacion == id).FirstOrDefault();
                return View(main);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult updatePublish(int id, string textArea, string firstName, HttpPostedFileBase img, HttpPostedFileBase doc)
        {
            var main = db.T_Publicaciones.Where(x => x.idPublicacion == id).FirstOrDefault();

            try
            {
                string pathImg = "";
                string pathDoc = "";
                if (img != null)
                {
                    if (img.ContentLength > 0)
                    {
                        var deletepath = Server.MapPath(main.imgRef);
                        if (System.IO.File.Exists(deletepath))
                        {
                            System.IO.File.Delete(deletepath);
                        }

                        string _FileName = Path.GetFileName(img.FileName);
                        string _path = Path.Combine(Server.MapPath("~/Imagenes"), _FileName);
                        string filename = fnalpath(_path);
                        string formato = _FileName.Split('.').Last();
                        string _path2 = Path.Combine(Server.MapPath("~/Imagenes"), filename + "." + formato);
                        pathImg = @"\Imagenes\" + filename + "." + formato;
                        img.SaveAs(_path2);
                        main.imgRef = pathImg;
                        db.SaveChanges();


                    }
                }
                if (doc != null)
                {
                    if (doc.ContentLength > 0)
                    {
                        var deletepath = Server.MapPath(main.docRef);
                        if (System.IO.File.Exists(deletepath))
                        {
                            System.IO.File.Delete(deletepath);
                        }

                        string _FileName = Path.GetFileName(doc.FileName);
                        _FileName.Split('.');
                        string _path = Path.Combine(Server.MapPath("~/Documents"), _FileName);
                        string filename = fnalpathDoc(_path);
                        string formato = _FileName.Split('.').Last();
                        string _path2 = Path.Combine(Server.MapPath("~/Documents"), filename + "." + formato);
                        pathDoc = @"\Documents\" + filename + "." + formato;
                        doc.SaveAs(_path2);
                        main.docRef = pathDoc;
                        db.SaveChanges();
                    }
                }
                main.Descripcion = textArea;
                main.Titulo = firstName;
                db.SaveChanges();
                TempData["Message"] = "Se modifico la publicacion";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Message"] = "Error al guardar archivos";
                return RedirectToAction("Index");
            }
        }
        public ActionResult deletePublicacion(int id)
        {
            var main = db.T_Publicaciones.Where(x => x.idPublicacion == id).FirstOrDefault();
            db.T_Publicaciones.Remove(main);
            db.SaveChanges();
            TempData["Message"] = "Se elimino la publicacion";
            return RedirectToAction("Index");
        }
        public ActionResult download(int id)
        {
            var main = db.T_Publicaciones.Where(x => x.idPublicacion == id).Select(x => x.docRef).FirstOrDefault();
            string file = @"c:\someFolder\foo.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(main, contentType, Path.GetFileName(main));
        }

        public Random a = new Random();
        private string fnalpath(string refe)
        {
            var validate = db.T_Publicaciones.Where(x => x.imgRef == refe).ToList();
            int MyNumber = 0;
            string finalpth = refe;
            if (validate.Count > 0)
            {
                var validation = db.T_Publicaciones.Select(x => x.imgRef).ToList();
                while (!validation.Contains(refe))
                    MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();

            }
            else
            {
                var validation = db.T_Publicaciones.Select(x => x.imgRef).ToList();
                MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();
            }

            return (finalpth);
        }
        private string fnalpathDoc(string refe)
        {
            var validate = db.T_Publicaciones.Where(x => x.docRef == refe).ToList();
            int MyNumber = 0;
            string finalpth = refe;
            if (validate.Count > 0)
            {
                var validation = db.T_Publicaciones.Select(x => x.docRef).ToList();
                while (!validation.Contains(refe))
                    MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();

            }
            else
            {
                var validation = db.T_Publicaciones.Select(x => x.docRef).ToList();
                MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();
            }

            return (finalpth);
        }
    }
}