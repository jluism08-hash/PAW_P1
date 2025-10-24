using PAW_P1.Data;
using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAW_P1.Controllers
{
    public class EvaluacionController : Controller
    {
        private readonly EvaluacionDAO evaluacionDao = new EvaluacionDAO();

        // GET: Evaluacion
        public ActionResult Index()
        {
            if (Session["Docente"] == null)
                return RedirectToAction("Login", "Login");
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Evaluacion evaluacion)
        {
            if (!ModelState.IsValid)
                return Json(new { ok = false, msg = "Datos inválidos." });

            var nuevoId = evaluacionDao.Insertar(evaluacion);
            return Json(new { ok = true, id = nuevoId });
        }

        [HttpGet]
        public ActionResult Buscar(string filtro)
        {
            var termino = filtro ?? string.Empty;
            var lista = evaluacionDao.Buscar(termino);
            return Json(new { ok = true, data = lista }, JsonRequestBehavior.AllowGet);
        }

    }
}