using PAW_P1.Data;
using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAW_P1.Controllers
{
    public class EstadisticaController : Controller
    {
        private readonly EstadisticaDAO estadisticasDao = new EstadisticaDAO();
        private readonly CuatrimestreDAO cuatriDao = new CuatrimestreDAO();
        private readonly CursoDAO cursoDao = new CursoDAO();

        // GET: /Estadistica
        public ActionResult Index(int? cuatrimestreId, int? cursoId)
        {
            if (Session["Docente"] == null)
                return RedirectToAction("Login", "Login");

            var cuatris = cuatriDao.Buscar("") ?? new List<Cuatrimestre>();
            var cursos = cursoDao.Buscar("") ?? new List<Curso>();

            ViewBag.Cuatrimestres = cuatris;
            ViewBag.Cursos = cursos;
            ViewBag.CuatrimestreId = cuatrimestreId;
            ViewBag.CursoId = cursoId;

            ViewBag.TotalEstudiantes = estadisticasDao.TotalEstudiantes(cuatrimestreId, cursoId);
            ViewBag.PromedioNotas = estadisticasDao.PromedioNotas(cuatrimestreId, cursoId);
            return View();
        }

        [HttpGet]
        public JsonResult CursosPorCuatrimestre(int? cuatrimestreId)
        {
            var datos = estadisticasDao.CursosPorCuatrimestre(cuatrimestreId) ?? new List<GraficoDato>();
            return Json(datos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EvaluacionesPorEstado(int? cuatrimestreId, int? cursoId)
        {
            var datos = estadisticasDao.EvaluacionesPorEstado(cuatrimestreId, cursoId) ?? new List<GraficoDato>();
            return Json(datos, JsonRequestBehavior.AllowGet);
        }
    }
}