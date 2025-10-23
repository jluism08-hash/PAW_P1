using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PAW_P1.Models
{
    public class Docente
    {
        public int IdDocente { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
    }
}