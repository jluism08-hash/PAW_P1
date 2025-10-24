using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAW_P1.Data;
using PAW_P1.Models;

namespace PAW_P1.Controllers
{

    public class CursoController : Controller
    {
        private readonly CursoDAO cursoDao = new CursoDAO();

        // GET: Curso
        public ActionResult Index()
        {
            if (Session["Docente"] == null)
                return RedirectToAction("Login", "Login");
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Curso curso)
        {
            if (!ModelState.IsValid)
                return Json(new { ok = false, msg = "Datos inválidos." });

            var nuevoId = cursoDao.Insertar(curso);
            return Json(new { ok = true, id = nuevoId });
        }

        [HttpGet]
        public ActionResult Buscar(string filtro)
        {
            var termino = filtro ?? string.Empty;
            var lista = cursoDao.Buscar(termino);
            return Json(new { ok = true, data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}