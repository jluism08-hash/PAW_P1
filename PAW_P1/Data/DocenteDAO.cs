using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PAW_P1.Data
{
    public class DocenteDAO
    {
        public int Insertar(Docente docente)
        {
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                INSERT INTO Docente (Usuario, Contrasena, Nombre, Correo)
                VALUES (@Usuario, @Contrasena, @Nombre, @Correo);
                SELECT SCOPE_IDENTITY();", connection))
            {
                command.Parameters.AddWithValue("@Usuario", docente.Usuario);
                command.Parameters.AddWithValue("@Contrasena", docente.Contrasena);
                command.Parameters.AddWithValue("@Nombre", docente.Nombre);
                command.Parameters.AddWithValue("@Correo", docente.Correo);

                var nuevoId = Convert.ToInt32(Convert.ToDecimal(command.ExecuteScalar()));
                return nuevoId;
            }
        }

        public List<Docente> Buscar(string textoBusqueda)
        {
            var lista = new List<Docente>();
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                SELECT IdDocente, Usuario, Contrasena, Nombre, Correo
                FROM Docente
                WHERE (@texto IS NULL OR @texto = '')
                   OR (Usuario LIKE @like OR Nombre LIKE @like OR Correo LIKE @like)
                ORDER BY Nombre;", connection))
            {
                command.Parameters.AddWithValue("@texto", textoBusqueda ?? string.Empty);
                command.Parameters.AddWithValue("@like", $"%{textoBusqueda}%");

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        lista.Add(new Docente
                        {
                            IdDocente = reader.GetInt32(0),
                            Usuario = reader.GetString(1),
                            Contrasena = reader.GetString(2),
                            Nombre = reader.GetString(3),
                            Correo = reader.GetString(4)
                        });
                    }
                }
            }
            return lista;
        }

        public Docente ObtenerPorUsuario(string usuario)
        {
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                SELECT TOP 1 IdDocente, Usuario, Contrasena, Nombre, Correo
                FROM Docente
                WHERE Usuario = @Usuario;", connection))
            {
                command.Parameters.AddWithValue("@Usuario", usuario);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    return new Docente
                    {
                        IdDocente = reader.GetInt32(0),
                        Usuario = reader.GetString(1),
                        Contrasena = reader.GetString(2),
                        Nombre = reader.GetString(3),
                        Correo = reader.GetString(4)
                    };
                }
            }
        }

        public Docente ValidarCredenciales(string usuario, string contrasena)
        {
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                SELECT TOP 1 IdDocente, Usuario, Contrasena, Nombre, Correo
                FROM Docente
                WHERE Usuario = @Usuario AND Contrasena = @Contrasena;", connection))
            {
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contrasena", contrasena);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    return new Docente
                    {
                        IdDocente = reader.GetInt32(0),
                        Usuario = reader.GetString(1),
                        Contrasena = reader.GetString(2),
                        Nombre = reader.GetString(3),
                        Correo = reader.GetString(4)
                    };
                }
            }
        }
    }
}