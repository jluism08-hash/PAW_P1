using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PAW_P1.Data
{
    public class EstudianteDAO
    {
        public bool ExisteDuplicado(string identificacion, string correo)
        {
            // Using para abrir y cerrar la conexión automáticamente
            using (var cn = Connection.GetConnection())
            using (var cmd = new SqlCommand(@"
                SELECT 1
                FROM Estudiante
                WHERE Identificacion = @Identificacion OR Correo = @Correo", cn))
            {
                cmd.Parameters.AddWithValue("@Identificacion", identificacion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", correo ?? (object)DBNull.Value);
                var result = cmd.ExecuteScalar();
                return result != null;
            }
        }

        public int Insertar(Estudiante e)
        {
            using (var cn = Connection.GetConnection())
            using (var cmd = new SqlCommand(@"
                INSERT INTO Estudiante
                (Nombre, Apellidos, Identificacion, FechaNacimiento, Provincia, Canton, Distrito, Correo)
                VALUES (@Nombre, @Apellidos, @Identificacion, @FechaNacimiento, @Provincia, @Canton, @Distrito, @Correo);
                SELECT SCOPE_IDENTITY();", cn))
            {
                cmd.Parameters.AddWithValue("@Nombre", e.Nombre);
                cmd.Parameters.AddWithValue("@Apellidos", e.Apellidos);
                cmd.Parameters.AddWithValue("@Identificacion", e.Identificacion);
                cmd.Parameters.AddWithValue("@FechaNacimiento", e.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Provincia", e.Provincia);
                cmd.Parameters.AddWithValue("@Canton", e.Canton);
                cmd.Parameters.AddWithValue("@Distrito", e.Distrito);
                cmd.Parameters.AddWithValue("@Correo", e.Correo);

                // Obtenemos el Id generado con SCOPE_IDENTITY()
                var id = Convert.ToInt32(Convert.ToDecimal(cmd.ExecuteScalar()));
                return id;
            }
        }

        public List<Estudiante> Buscar(string filtro)
        {
            var lista = new List<Estudiante>();
            using (var cn = Connection.GetConnection())
            using (var cmd = new SqlCommand(@"
                SELECT IdEstudiante, Nombre, Apellidos, Identificacion, FechaNacimiento,
                       Provincia, Canton, Distrito, Correo
                FROM Estudiante
                WHERE (@filtro IS NULL OR @filtro = '')
                   OR (Nombre LIKE @like OR Apellidos LIKE @like OR Identificacion LIKE @like)
                ORDER BY Apellidos, Nombre;", cn))
            {
                cmd.Parameters.AddWithValue("@filtro", filtro ?? string.Empty);
                cmd.Parameters.AddWithValue("@like", $"%{filtro}%");

                using (var rd = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (rd.Read())
                    {
                        lista.Add(new Estudiante
                        {
                            IdEstudiante = rd.GetInt32(0),
                            Nombre = rd.GetString(1),
                            Apellidos = rd.GetString(2),
                            Identificacion = rd.GetString(3),
                            FechaNacimiento = rd.GetDateTime(4),
                            Provincia = rd.GetString(5),
                            Canton = rd.GetString(6),
                            Distrito = rd.GetString(7),
                            Correo = rd.GetString(8)
                        });
                    }
                }
            }
            return lista;
        }
    }
}