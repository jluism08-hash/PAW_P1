using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PAW_P1.Data
{
    public class CursoDAO
    {
        public int Insertar(Curso curso)
        {
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                INSERT INTO Curso (Nombre, IdCuatrimestre)
                VALUES (@Nombre, @IdCuatrimestre);
                SELECT SCOPE_IDENTITY();", connection))
            {
                command.Parameters.AddWithValue("@Nombre", curso.Nombre);
                command.Parameters.AddWithValue("@IdCuatrimestre", curso.IdCuatrimestre);
                var nuevoId = Convert.ToInt32(Convert.ToDecimal(command.ExecuteScalar()));
                return nuevoId;
            }
        }

        public List<Curso> Buscar(string textoBusqueda)
        {
            var lista = new List<Curso>();
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                SELECT C.IdCurso, C.Nombre, C.IdCuatrimestre, Q.Nombre AS NombreCuatrimestre
                FROM Curso C
                INNER JOIN Cuatrimestre Q ON C.IdCuatrimestre = Q.IdCuatrimestre
                WHERE (@texto IS NULL OR @texto = '')
                   OR (C.Nombre LIKE @like OR Q.Nombre LIKE @like)
                ORDER BY Q.Nombre, C.Nombre;", connection))
            {
                command.Parameters.AddWithValue("@texto", textoBusqueda ?? string.Empty);
                command.Parameters.AddWithValue("@like", $"%{textoBusqueda}%");

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        lista.Add(new Curso
                        {
                            IdCurso = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            IdCuatrimestre = reader.GetInt32(2)
                        });
                    }
                }
            }
            return lista;
        }
    }
}