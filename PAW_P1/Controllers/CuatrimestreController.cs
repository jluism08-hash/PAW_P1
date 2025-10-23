using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAW_P1.Data;

namespace PAW_P1.Models
{
    public class CuatrimestreController : Controller
    {
        private readonly CuatrimestreDAO cuatrimestreDao = new CuatrimestreDAO();

        // GET: Cuatrimestre
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Cuatrimestre cuatrimestre)
        {
            if (!ModelState.IsValid)
                return Json(new { ok = false, msg = "Datos inválidos." });

            var nuevoId = cuatrimestreDao.Insertar(cuatrimestre);
            return Json(new { ok = true, id = nuevoId });
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