using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DobleCiego.Models;

namespace DobleCiego.Controllers
{ 
    [Authorize]
    public class AdminUsersController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        // GET: AdminUsers
        public ActionResult Index()
        {
            var main = db.T_Usuarios.Where(x => x.Status == "Activo").ToList();
            return View(main);
        }
        public ActionResult addUser()
        {
            return View();
        }
        public ActionResult createUser(string firstName, string lastName, DateTime bday, string email, string phone, string password1, string Rol, string genero) 
        {
            var correo = db.T_Usuarios.Where(x => x.Correo == email).FirstOrDefault();

          if (correo == null)
                {
                    int edad = DateTime.Today.Year - bday.Year;
                    var main = new T_Usuarios();
                    main.Nombre = firstName;
                    main.Apellido = lastName;
                    main.Rol = Rol;
                    main.Status = "Activo";
                    main.Password = password1;
                    main.FechaNac = bday;
                    main.Telefono = phone;
                    main.Correo = email;
                    main.Sexo = genero;
                    db.T_Usuarios.Add(main);
                    db.SaveChanges();
                TempData["Message"] = "Usuario agregado exitosamente";
                return RedirectToAction("Index", "AdminUsers");
                }
                else
                {
                    TempData["Message"] = "Correo en uso";
                    return RedirectToAction("Index", "AdminUsers");
                }

        }
        public ActionResult modifyUser(int? id)
        {
            if (id != null)
            {
                var main = db.T_Usuarios.Where(x => x.IdUsuario == id).FirstOrDefault();
                return View(main);
            }
            else
            {
                return RedirectToAction("Index", "AdminUsers");
            }
          
        }
        public ActionResult modificarUsuario(int id, string firstName, string lastName, DateTime bday, string email, string phone, string password1, string Rol, string genero) 
        {
            var main = db.T_Usuarios.Where(x => x.IdUsuario == id).FirstOrDefault();

            var maincorreo = db.T_Usuarios.Where(x => x.IdUsuario != id).Select(x => x.Correo).ToList();
            if (!maincorreo.Contains(email))
            {
                main.Nombre = firstName;
                main.Apellido = lastName;
                main.Rol = Rol;
                main.Status = "Activo";
                main.Password = password1;
                main.FechaNac = bday;
                main.Telefono = phone;
                main.Correo = email;
                main.Sexo = genero;
                db.SaveChanges();
                TempData["Message"] = "Se modifico el usuario";
                return RedirectToAction("Index", "AdminUsers");
            }
            else
            {
                TempData["Message"] = "Correo en uso";
                return RedirectToAction("Index", "AdminUsers");
            }

        }
        public ActionResult deleteUser(int id)
        {
            var main = db.T_Usuarios.Where(x => x.IdUsuario == id).FirstOrDefault();
            db.T_Usuarios.Remove(main);
            db.SaveChanges();
            return RedirectToAction("Index", "AdminUsers");
        }
    }
}