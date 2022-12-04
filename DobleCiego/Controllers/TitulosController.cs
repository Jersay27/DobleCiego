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
    public class TitulosController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        // GET: Titulos
        public ActionResult Index()
        {
            var main = db.T_Titulos.ToList();
            var mainImage = db.T_Titulos.Select(x => x.ImagenRef).ToList();

            return View(main);
        }
        public ActionResult download(int id)
        {
            var main = db.T_Titulos.Where(x => x.IdTitulo == id).Select(x => x.DocRef).FirstOrDefault(); 
            string file = @"c:\someFolder\foo.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(main, contentType, Path.GetFileName(main));
        }
        public ActionResult crearTitulo() 
        {
            return View();
        }
        [HttpPost]
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
                var main = new T_Titulos();
                DateTime actualdate =  DateTime.Now;
                main.idUsuario = idUsuario;
                main.Propuesta = textArea;
                main.Titulo = firstName;
                main.DocRef = pathDoc;
                main.ImagenRef = pathImg;
                main.CreationDate = actualdate;
                main.Status = "Creado";
                db.T_Titulos.Add(main);
                db.SaveChanges();
                TempData["Message"] = "Se aggrego un nuevo titulo";
                return RedirectToAction("Index", "Titulos");
            }
            catch
            {
                TempData["Message"] = "Error al guardar archivos";
                return RedirectToAction("Index", "Titulos");
            }

        }

        public ActionResult ModificarTitulo(int? id) 
        {
            if (id != null)
            {
                var main = db.T_Titulos.Where(x => x.IdTitulo == id).FirstOrDefault();
                return View(main);
            }
            else
            {
                return RedirectToAction("Index", "Titulos");
            }
   
        }
        public ActionResult updateTitle(int id, string textArea, string firstName, HttpPostedFileBase img, HttpPostedFileBase doc)
        {
            var main = db.T_Titulos.Where(x => x.IdTitulo == id).FirstOrDefault();

            try
            {
                string pathImg = "";
                string pathDoc = "";
                if (img != null)
                {
                    if (img.ContentLength > 0)
                    {
                        var deletepath = Server.MapPath(main.ImagenRef);
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
                        main.ImagenRef = pathImg;
                        db.SaveChanges();

 
                    }
                }
                if (doc != null)
                {
                    if (doc.ContentLength > 0)
                    {
                        var deletepath = Server.MapPath(main.DocRef);
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
                        main.DocRef = pathDoc;
                        db.SaveChanges();
                    }
                }
                main.Propuesta = textArea;
                main.Titulo = firstName;
                db.SaveChanges();
                TempData["Message"] = "Se modifico el titulo";
                return RedirectToAction("Index", "Titulos");
            }
            catch
            {
                TempData["Message"] = "Error al guardar archivos";
                return RedirectToAction("Index", "Titulos");
            }
        }
        public ActionResult deleteTitle(int id)
        {
            try
            {
                var main = db.T_Titulos.Where(x => x.IdTitulo == id).FirstOrDefault();
                var deletepath = Server.MapPath(main.ImagenRef);
                if (System.IO.File.Exists(deletepath))
                {
                    System.IO.File.Delete(deletepath);
                }
                var deletepath2 = Server.MapPath(main.DocRef);
                if (System.IO.File.Exists(deletepath2))
                {
                    System.IO.File.Delete(deletepath2);
                }

                db.T_Titulos.Remove(main);
                db.SaveChanges();
                TempData["Message"] = "Se elimino el titulo";

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Titulos");
            }


            return RedirectToAction("Index", "Titulos");
        }
        public ActionResult finTitle(int id) 
        {
            var main = db.T_Titulos.Where(x => x.IdTitulo == id).FirstOrDefault();
            main.Status = "Finalizado";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public Random a = new Random(); 
        private string fnalpath(string refe)
        {
            var validate = db.T_Titulos.Where(x => x.ImagenRef == refe).ToList();
            int MyNumber = 0;
            string finalpth = refe;
            if (validate.Count > 0)
            {
                var validation = db.T_Titulos.Select(x => x.ImagenRef).ToList();
                while (!validation.Contains(refe))
                MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();

            }
            else
            {
                var validation = db.T_Titulos.Select(x => x.ImagenRef).ToList();
                MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();
            }

            return (finalpth);
        }
        private string fnalpathDoc(string refe)
        {
            var validate = db.T_Titulos.Where(x => x.DocRef == refe).ToList();
            int MyNumber = 0;
            string finalpth = refe;
            if (validate.Count > 0)
            {
                var validation = db.T_Titulos.Select(x => x.DocRef).ToList();
                while (!validation.Contains(refe))
                    MyNumber = a.Next(0, 10000);
                finalpth =  MyNumber.ToString();

            }
            else
            {
                var validation = db.T_Titulos.Select(x => x.DocRef).ToList();
                MyNumber = a.Next(0, 10000);
                finalpth = MyNumber.ToString();
            }

            return (finalpth);
        }

    }
}