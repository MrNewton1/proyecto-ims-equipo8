using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Domain;

namespace Infrastructure
{
    /// <summary>
    /// Contexto de acceso a datos para la entidad Préstamo,
    /// implementando operaciones CRUD mediante ADO.NET.
    /// </summary>
    public class PrestamosDbContext
    {
        /// <summary>
        /// Cadena de conexión a la base de datos.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="PrestamosDbContext"/> con la cadena de conexión especificada.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión para la base de datos.</param>
        public PrestamosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Recupera todos los registros de préstamos de la base de datos.
        /// </summary>
        /// <returns>Lista de objetos <see cref="IM253E08Prestamo"/>.</returns>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la consulta o leer los datos.</exception>
        public List<IM253E08Prestamo> List()
        {
            var prestamos = new List<IM253E08Prestamo>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT Id, UsuarioId, LibroId, FechaPrestamo, FechaDevolucion FROM IM253E08Prestamos",
                connection);
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    prestamos.Add(new IM253E08Prestamo
                    {
                        Id = reader.GetGuid(0),
                        UsuarioId = reader.GetGuid(1),
                        LibroId = reader.GetGuid(2),
                        FechaPrestamo = reader.GetDateTime(3),
                        FechaDevolucion = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
                    });
                }
                return prestamos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving prestamos from database", ex);
            }
        }

        /// <summary>
        /// Obtiene los detalles de un préstamo específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del préstamo.</param>
        /// <returns>Objeto <see cref="IM253E08Prestamo"/> con los datos del préstamo.</returns>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la consulta o leer los datos.</exception>
        public IM253E08Prestamo Details(Guid id)
        {
            var prestamo = new IM253E08Prestamo();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT Id, UsuarioId, LibroId, FechaPrestamo, FechaDevolucion FROM IM253E08Prestamos WHERE Id = @Id",
                connection);
            command.Parameters.Add(new SqlParameter("@Id", id));
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    prestamo.Id = reader.GetGuid(0);
                    prestamo.UsuarioId = reader.GetGuid(1);
                    prestamo.LibroId = reader.GetGuid(2);
                    prestamo.FechaPrestamo = reader.GetDateTime(3);
                    prestamo.FechaDevolucion = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                }
                return prestamo;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving prestamo details", ex);
            }
        }

        /// <summary>
        /// Inserta un nuevo registro de préstamo en la base de datos.
        /// </summary>
        /// <param name="data">Objeto <see cref="IM253E08Prestamo"/> con los datos del préstamo.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la inserción.</exception>
        public void Create(IM253E08Prestamo data)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "INSERT INTO IM253E08Prestamos (Id, UsuarioId, LibroId, FechaPrestamo, FechaDevolucion) VALUES (@Id, @UsuarioId, @LibroId, @FechaPrestamo, @FechaDevolucion)",
                connection);
            command.Parameters.Add(new SqlParameter("@Id", data.Id));
            command.Parameters.Add(new SqlParameter("@UsuarioId", data.UsuarioId));
            command.Parameters.Add(new SqlParameter("@LibroId", data.LibroId));
            command.Parameters.Add(new SqlParameter("@FechaPrestamo", data.FechaPrestamo));
            command.Parameters.Add(new SqlParameter("@FechaDevolucion", (object?)data.FechaDevolucion ?? DBNull.Value));
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating prestamo", ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de un préstamo existente.
        /// </summary>
        /// <param name="data">Objeto <see cref="IM253E08Prestamo"/> con los datos actualizados.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la actualización.</exception>
        public void Edit(IM253E08Prestamo data)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "UPDATE IM253E08Prestamos SET UsuarioId = @UsuarioId, LibroId = @LibroId, FechaPrestamo = @FechaPrestamo, FechaDevolucion = @FechaDevolucion WHERE Id = @Id",
                connection);
            command.Parameters.Add(new SqlParameter("@UsuarioId", data.UsuarioId));
            command.Parameters.Add(new SqlParameter("@LibroId", data.LibroId));
            command.Parameters.Add(new SqlParameter("@FechaPrestamo", data.FechaPrestamo));
            command.Parameters.Add(new SqlParameter("@FechaDevolucion", (object?)data.FechaDevolucion ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Id", data.Id));
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error editing prestamo", ex);
            }
        }

        /// <summary>
        /// Elimina un préstamo de la base de datos según su identificador.
        /// </summary>
        /// <param name="id">Identificador del préstamo a eliminar.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la eliminación.</exception>
        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "DELETE FROM IM253E08Prestamos WHERE Id = @Id",
                connection);
            command.Parameters.Add(new SqlParameter("@Id", id));
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting prestamo", ex);
            }
        }
    }
}
