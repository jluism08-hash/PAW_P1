using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PAW_P1.Data
{
    public class EvaluacionDAO
    {
        public int Insertar(Evaluacion evaluacion)
        {
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                INSERT INTO Evaluacion
                (IdEstudiante, IdCurso, Nota, Observaciones, Participacion, Estado)
                VALUES (@IdEstudiante, @IdCurso, @Nota, @Observaciones, @Participacion, @Estado);
                SELECT SCOPE_IDENTITY();", connection))
            {
                command.Parameters.AddWithValue("@IdEstudiante", evaluacion.IdEstudiante);
                command.Parameters.AddWithValue("@IdCurso", evaluacion.IdCurso);
                command.Parameters.AddWithValue("@Nota", evaluacion.Nota);
                command.Parameters.AddWithValue("@Observaciones", evaluacion.Observaciones ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Participacion", evaluacion.Participacion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Estado", evaluacion.Estado ?? (object)DBNull.Value);

                var nuevoId = Convert.ToInt32(Convert.ToDecimal(command.ExecuteScalar()));
                return nuevoId;
            }
        }

        public List<Evaluacion> Buscar(string textoBusqueda)
        {
            var lista = new List<Evaluacion>();
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                SELECT E.IdEvaluacion, Es.Nombre AS NombreEstudiante, C.Nombre AS NombreCurso,
                       E.Nota, E.Observaciones, E.Participacion, E.Estado
                FROM Evaluacion E
                INNER JOIN Estudiante Es ON E.IdEstudiante = Es.IdEstudiante
                INNER JOIN Curso C ON E.IdCurso = C.IdCurso
                WHERE (@texto IS NULL OR @texto = '')
                   OR (Es.Nombre LIKE @like OR C.Nombre LIKE @like)
                ORDER BY E.IdEvaluacion DESC;", connection))
            {
                command.Parameters.AddWithValue("@texto", textoBusqueda ?? string.Empty);
                command.Parameters.AddWithValue("@like", $"%{textoBusqueda}%");

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        lista.Add(new Evaluacion
                        {
                            IdEvaluacion = reader.GetInt32(0),
                            Observaciones = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            Participacion = reader.IsDBNull(5) ? "" : reader.GetString(5),
                            Estado = reader.IsDBNull(6) ? "" : reader.GetString(6)
                        });
                    }
                }
            }
            return lista;
        }
    }
}