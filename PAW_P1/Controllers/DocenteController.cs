using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAW_P1.Data;

namespace PAW_P1.Controllers
{
    public class DocenteController : Controller
    {
        private readonly DocenteDAO docenteDao = new DocenteDAO();

        // GET: Docente
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Docente docente)
        {
            if (!ModelState.IsValid)
                return Json(new { ok = false, msg = "Datos inválidos." });

            var nuevoId = docenteDao.Insertar(docente);
            return Json(new { ok = true, id = nuevoId });
        }

        [HttpGet]
        public ActionResult Buscar(string filtro)
        {
            var termino = filtro ?? string.Empty;
            var lista = docenteDao.Buscar(termino);
            return Json(new { ok = true, data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}