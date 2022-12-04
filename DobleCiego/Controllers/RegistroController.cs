using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DobleCiego.Models;

namespace DobleCiego.Controllers
{
    public class RegistroController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        // GET: Registro
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar(string firstName, string lastName, DateTime bday, string email, string phone, string password1, string password2, string genero)
        {
            var correo = db.T_Usuarios.Where(x => x.Correo == email).FirstOrDefault();

            if (password1 == password2)
            {
                if (correo == null)
                {
                    int edad = DateTime.Today.Year - bday.Year;
                    var main = new T_Usuarios();
                    main.Nombre = firstName;
                    main.Apellido = lastName;
                    main.Rol = "Escritor";
                    main.Status = "Activo";
                    main.Password = password1;
                    main.FechaNac = bday;
                    main.Telefono = phone;
                    main.Correo = email;
                    main.Sexo = genero;
                    db.T_Usuarios.Add(main);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Message"] = "Correo en uso";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Message"] = "Contraseñas no coinciden";
                return RedirectToAction("Index");
            }


        }
    }
}