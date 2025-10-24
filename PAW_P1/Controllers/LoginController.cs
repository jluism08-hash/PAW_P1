using PAW_P1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAW_P1.Controllers
{
    public class LoginController : Controller
    {
        private readonly DocenteDAO docenteDao = new DocenteDAO();

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string contrasena)
        {
            var docente = docenteDao.ValidarCredenciales(usuario, contrasena);
            if (docente != null)
            {
                Session["Docente"] = docente.Nombre;   // simple para P1
                Session["DocenteId"] = docente.IdDocente;
                return RedirectToAction("Index", "Estudiante");
            }

            ViewBag.Mensaje = "Usuario o contraseña incorrectos.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}