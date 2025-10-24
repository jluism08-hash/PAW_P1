using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PAW_P1.Data
{
    public class EstadisticaDAO
    {
        public int TotalEstudiantes()
        {
            using (var cn = Connection.GetConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Estudiante;", cn))
                return (int)cmd.ExecuteScalar();
        }

        public decimal PromedioNotas(int? cuatrimestreId, int? cursoId)
        {
            var sql = @"
                        SELECT AVG(CAST(Ev.Nota AS DECIMAL(10,2)))
                        FROM Evaluacion Ev
                        INNER JOIN Curso C ON Ev.IdCurso = C.IdCurso
                        WHERE (@cId IS NULL OR C.IdCuatrimestre = @cId)
                          AND (@cursoId IS NULL OR Ev.IdCurso = @cursoId)";
            using (var cn = Connection.GetConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@cId", (object)cuatrimestreId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cursoId", (object)cursoId ?? DBNull.Value);
                var v = cmd.ExecuteScalar();
                return v == DBNull.Value ? 0m : Convert.ToDecimal(v);
            }
        }

        public List<GraficoDato> CursosPorCuatrimestre(int? cuatrimestreId)
        {
            var lista = new List<GraficoDato>();
            var sql = @"
                        SELECT Q.Nombre, COUNT(*) Cantidad
                        FROM Curso C INNER JOIN Cuatrimestre Q ON C.IdCuatrimestre = Q.IdCuatrimestre
                        WHERE (@cId IS NULL OR C.IdCuatrimestre = @cId)
                        GROUP BY Q.Nombre
                        ORDER BY Q.Nombre;";
            using (var cn = Connection.GetConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@cId", (object)cuatrimestreId ?? DBNull.Value);
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read())
                        lista.Add(new GraficoDato { Etiqueta = rd.GetString(0), Valor = rd.GetInt32(1) });
            }
            return lista;
        }

        public List<GraficoDato> EvaluacionesPorEstado(int? cuatrimestreId, int? cursoId)
        {
            var lista = new List<GraficoDato>();
            var sql = @"
                        SELECT ISNULL(NULLIF(LTRIM(RTRIM(Ev.Estado)) ,''),'Sin estado') AS Estado, COUNT(*) Cantidad
                        FROM Evaluacion Ev
                        INNER JOIN Curso C ON Ev.IdCurso = C.IdCurso
                        WHERE (@cId IS NULL OR C.IdCuatrimestre = @cId)
                          AND (@cursoId IS NULL OR Ev.IdCurso = @cursoId)
                        GROUP BY ISNULL(NULLIF(LTRIM(RTRIM(Ev.Estado)) ,''),'Sin estado')
                        ORDER BY 1;";
            using (var cn = Connection.GetConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@cId", (object)cuatrimestreId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cursoId", (object)cursoId ?? DBNull.Value);
                using (var rd = cmd.ExecuteReader())
                    while (rd.Read())
                        lista.Add(new GraficoDato { Etiqueta = rd.GetString(0), Valor = rd.GetInt32(1) });
            }
            return lista;
        }

        public int TotalEstudiantes(int? cuatrimestreId, int? cursoId)
        {
            // Total estudiantes con evaluaciones filtradas (si no hay filtros, total global)
            var sql = @"
                        SELECT COUNT(DISTINCT Es.IdEstudiante)
                        FROM Estudiante Es
                        LEFT JOIN Evaluacion Ev ON Ev.IdEstudiante = Es.IdEstudiante
                        LEFT JOIN Curso C ON Ev.IdCurso = C.IdCurso
                        WHERE (@cId IS NULL OR C.IdCuatrimestre = @cId)
                          AND (@cursoId IS NULL OR Ev.IdCurso = @cursoId)";
            using (var cn = Connection.GetConnection())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@cId", (object)cuatrimestreId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cursoId", (object)cursoId ?? DBNull.Value);
                var v = cmd.ExecuteScalar();
                return v == DBNull.Value ? 0 : Convert.ToInt32(v);
            }
        }
    }
}