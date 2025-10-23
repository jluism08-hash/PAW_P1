using PAW_P1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PAW_P1.Data
{
    public class CuatrimestreDAO
    {
        public int Insertar(Cuatrimestre cuatrimestre)
        {
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                INSERT INTO Cuatrimestre (Nombre)
                VALUES (@Nombre);
                SELECT SCOPE_IDENTITY();", connection))
            {
                command.Parameters.AddWithValue("@Nombre", cuatrimestre.Nombre);
                var nuevoId = Convert.ToInt32(Convert.ToDecimal(command.ExecuteScalar()));
                return nuevoId;
            }
        }

        public List<Cuatrimestre> Buscar(string textoBusqueda)
        {
            var lista = new List<Cuatrimestre>();
            using (var connection = Connection.GetConnection())
            using (var command = new SqlCommand(@"
                SELECT IdCuatrimestre, Nombre
                FROM Cuatrimestre
                WHERE (@texto IS NULL OR @texto = '')
                   OR (Nombre LIKE @like)
                ORDER BY IdCuatrimestre;", connection))
            {
                command.Parameters.AddWithValue("@texto", textoBusqueda ?? string.Empty);
                command.Parameters.AddWithValue("@like", $"%{textoBusqueda}%");

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        lista.Add(new Cuatrimestre
                        {
                            IdCuatrimestre = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        });
                    }
                }
            }
            return lista;
        }
    }
}