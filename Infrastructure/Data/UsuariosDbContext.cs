using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Domain;

namespace Infrastructure
{
    /// <summary>
    /// Contexto de acceso a datos para la entidad Usuario,
    /// implementando operaciones CRUD mediante ADO.NET.
    /// </summary>
    public class UsuariosDbContext
    {
        /// <summary>
        /// Cadena de conexión a la base de datos.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="UsuariosDbContext"/> con la cadena de conexión proporcionada.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión para conectarse a la base de datos.</param>
        public UsuariosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Recupera todos los usuarios almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de objetos <see cref="IM253E08Usuario"/>.</returns>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la consulta o leer los datos.</exception>
        public List<IM253E08Usuario> List()
        {
            var usuarios = new List<IM253E08Usuario>();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT [Id], [Nombre], [Direccion], [Telefono], [Correo] FROM IM253E08Usuario", connection);
            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    usuarios.Add(new IM253E08Usuario
                    {
                        Id = reader.GetGuid(0),
                        Nombre = reader.GetString(1),
                        Direccion = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Telefono = reader.GetString(3),
                        Correo = reader.GetString(4)
                    });
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving usuarios from database", ex);
            }
        }

        /// <summary>
        /// Obtiene los detalles de un usuario específico por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del usuario.</param>
        /// <returns>Objeto <see cref="IM253E08Usuario"/> con los datos del usuario.</returns>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la consulta o leer los datos.</exception>
        public IM253E08Usuario Details(Guid id)
        {
            var usuario = new IM253E08Usuario();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "SELECT [Id], [Nombre], [Direccion], [Telefono], [Correo] FROM IM253E08Usuario WHERE Id = @Id", connection);
            command.Parameters.Add(new SqlParameter("@Id", id));

            try
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario.Id = reader.GetGuid(0);
                    usuario.Nombre = reader.GetString(1);
                    usuario.Direccion = reader.IsDBNull(2) ? null : reader.GetString(2);
                    usuario.Telefono = reader.GetString(3);
                    usuario.Correo = reader.GetString(4);
                }
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving usuario details", ex);
            }
        }

        /// <summary>
        /// Inserta un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="data">Objeto <see cref="IM253E08Usuario"/> con la información del usuario a crear.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la inserción.</exception>
        public void Create(IM253E08Usuario data)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "INSERT INTO IM253E08Usuario (Id, Nombre, Direccion, Telefono, Correo) VALUES (@Id, @Nombre, @Direccion, @Telefono, @Correo)", connection);

            command.Parameters.Add(new SqlParameter("@Id", data.Id));
            command.Parameters.Add(new SqlParameter("@Nombre", data.Nombre));
            command.Parameters.Add(new SqlParameter("@Direccion", (object?)data.Direccion ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Telefono", data.Telefono));
            command.Parameters.Add(new SqlParameter("@Correo", data.Correo));

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating usuario", ex);
            }
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente en la base de datos.
        /// </summary>
        /// <param name="data">Objeto <see cref="IM253E08Usuario"/> con los datos actualizados del usuario.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la actualización.</exception>
        public void Edit(IM253E08Usuario data)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "UPDATE IM253E08Usuario SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, Correo = @Correo WHERE Id = @Id", connection);

            command.Parameters.Add(new SqlParameter("@Nombre", data.Nombre));
            command.Parameters.Add(new SqlParameter("@Direccion", (object?)data.Direccion ?? DBNull.Value));
            command.Parameters.Add(new SqlParameter("@Telefono", data.Telefono));
            command.Parameters.Add(new SqlParameter("@Correo", data.Correo));
            command.Parameters.Add(new SqlParameter("@Id", data.Id));

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error editing usuario", ex);
            }
        }

        /// <summary>
        /// Elimina un usuario de la base de datos por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del usuario a eliminar.</param>
        /// <exception cref="Exception">Si ocurre un error al ejecutar la eliminación.</exception>
        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(
                "DELETE FROM IM253E08Usuario WHERE Id = @Id", connection);
            command.Parameters.Add(new SqlParameter("@Id", id));

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting usuario", ex);
            }
        }
    }
}
