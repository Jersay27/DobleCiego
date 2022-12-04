using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DobleCiego.Models;

namespace DobleCiego.Controllers
{
    public class LoginController : Controller
    {
        DobleCiegoEntities db = new DobleCiegoEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Validate(string email, string password)
        {
            if (email != null & password != null)
            {
                var main = db.T_Usuarios.Where(x => x.Correo == email & x.Password == password).FirstOrDefault();
                if (main != null)
                {
                    FormsAuthentication.SetAuthCookie( Convert.ToString(main.IdUsuario), true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    LogOut();
                    return RedirectToAction("Index");

                }
            }
            else
            {
                LogOut();
                return RedirectToAction("Index");

            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}