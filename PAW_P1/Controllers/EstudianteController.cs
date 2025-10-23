using PAW_P1.Data;
using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAW_P1.Controllers
{
    public class EstudianteController : Controller
    {
        private readonly EstudianteDAO dao = new EstudianteDAO();

        // GET: Estudiante
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Estudiante estudiante)
        {
            if (!ModelState.IsValid)
                return Json(new { ok = false, msg = "Datos inválidos." });

            if (dao.ExisteDuplicado(estudiante.Identificacion, estudiante.Correo))
                return Json(new { ok = false, msg = "Identificación o correo ya existen." });

            var id = dao.Insertar(estudiante);
            return Json(new { ok = true, id });
        }

        [HttpGet]
        public ActionResult Buscar(string filtro)
        {
            var termino = filtro ?? string.Empty;
            var lista = dao.Buscar(termino);
            return Json(new { ok = true, data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}