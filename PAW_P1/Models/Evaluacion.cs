using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAW_P1.Models
{
    public class Evaluacion
    {
        public int IdEvaluacion { get; set; }
        public int IdEstudiante { get; set; }
        public int IdCurso { get; set; }
        public string NombreEstudiante { get; set; }
        public string NombreCurso { get; set; }
        public decimal Nota { get; set; }
        public string Observaciones { get; set; }
        public string Participacion { get; set; }
        public string Estado { get; set; }
    }
}