using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Domain;

namespace Infrastructure
{
    /// <summary>
    /// Contexto de acceso a datos para la entidad Libro, implementando operaciones CRUD mediante ADO.NET.
    /// </summary>
    public class LibrosDbContext
    {
        /// <summary>
        /// Cadena de conexión a la base de datos.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="LibrosDbContext"/> con la cadena de conexión proporcionada.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión para conectarse a la base de datos.</param>
        public LibrosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Recupera todos los libros almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de objetos <see cref="IM253E08Libro"/> con los datos de cada libro.</returns>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la consulta o al leer los datos.</exception>
        public List<IM253E08Libro> List()
        {
            var libros = new List<IM253E08Libro>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT [Id], [Autor], [Editorial], [ISBN], [Foto] FROM IM253E08Libro",
                connection);
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    libros.Add(new IM253E08Libro
                    {
                        Id = reader.GetGuid(0),
                        Autor = reader.GetString(1),
                        Editorial = reader.IsDBNull(2) ? null : reader.GetString(2),
                        ISBN = reader.GetString(3),
                        Foto = reader.IsDBNull(4) ? null : reader.GetString(4)
                    });
                }
                return libros;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving libros from database", ex);
            }
        }

        /// <summary>
        /// Obtiene los detalles de un libro específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del libro.</param>
        /// <returns>Objeto <see cref="IM253E08Libro"/> con los datos del libro. Si no existe, devuelve una instancia con valores por defecto.</returns>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la consulta o al leer los datos.</exception>
        public IM253E08Libro Details(Guid id)
        {
            var libro = new IM253E08Libro();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT [Id], [Autor], [Editorial], [ISBN], [Foto] FROM IM253E08Libro WHERE Id = @Id",
                connection);
            command.Parameters.Add(new SqlParameter("@Id", id));

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    libro.Id = reader.GetGuid(0);
                    libro.Autor = reader.GetString(1);
                    libro.Editorial = reader.IsDBNull(2) ? null : reader.GetString(2);
                    libro.ISBN = reader.GetString(3);
                    libro.Foto = reader.IsDBNull(4) ? null : reader.GetString(4);
                }
                return libro;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving libro details", ex);
            }
        }

        /// <summary>
        /// Inserta un nuevo libro en la base de datos.
        /// </summary>
        /// <param name="data">Objeto <see cref="IM253E08Libro"/> con la información del libro a crear.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar el comando de inserción.</exception>
        public void Create(IM253E08Libro data)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "INSERT INTO IM253E08Libro (Id, Autor, Editorial, ISBN, Foto) VALUES (@Id, @Autor, @Editorial, @ISBN, @Foto)",
                connection);

            command.Parameters.Add(new SqlParameter("@Id", data.Id));
            command.Parameters.Add(new SqlParameter("@Autor", data.Autor));
            command.Parameters.Add(new SqlParameter("@Editorial", (object?)data.Editorial ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ISBN", data.ISBN));
            command.Parameters.Add(new SqlParameter("@Foto", (object?)data.Foto ?? DBNull.Value));

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating libro", ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de un libro existente en la base de datos.
        /// </summary>
        /// <param name="data">Objeto <see cref="IM253E08Libro"/> con los datos actualizados del libro.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar el comando de actualización.</exception>
        public void Edit(IM253E08Libro data)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "UPDATE IM253E08Libro SET Autor = @Autor, Editorial = @Editorial, ISBN = @ISBN, Foto = @Foto WHERE Id = @Id",
                connection);

            command.Parameters.Add(new SqlParameter("@Autor", data.Autor));
            command.Parameters.Add(new SqlParameter("@Editorial", (object?)data.Editorial ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@ISBN", data.ISBN));
            command.Parameters.Add(new SqlParameter("@Foto", (object?)data.Foto ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Id", data.Id));

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error editing libro", ex);
            }
        }

        /// <summary>
        /// Elimina un libro de la base de datos por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del libro a eliminar.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar el comando de eliminación.</exception>
        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "DELETE FROM IM253E08Libro WHERE Id = @Id",
                connection);
            command.Parameters.Add(new SqlParameter("@Id", id));

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting libro", ex);
            }
        }
    }
}
