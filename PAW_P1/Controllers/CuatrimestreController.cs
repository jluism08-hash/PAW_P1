using PAW_P1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PAW_P1.Models
{
    public class CuatrimestreController : Controller
    {
        private readonly CuatrimestreDAO cuatrimestreDao = new CuatrimestreDAO();

        // GET: Cuatrimestre
        public ActionResult Index()
        {
            if (Session["Docente"] == null)
                return RedirectToAction("Login", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult Crear(Cuatrimestre cuatrimestre)
        {
            /*if (!ModelState.IsValid)
                return Json(new { ok = false, msg = "Datos inválidos." });

            var nuevoId = cuatrimestreDao.Insertar(cuatrimestre);
            return Json(new { ok = true, id = nuevoId });*/
            try
            {
                var nuevoId = cuatrimestreDao.Insertar(cuatrimestre);
                return Json(new { ok = true, id = nuevoId });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { ok = false, msg = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Buscar(string filtro)
        {
            var termino = filtro ?? string.Empty;
            var lista = cuatrimestreDao.Buscar(termino);
            return Json(new { ok = true, data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}