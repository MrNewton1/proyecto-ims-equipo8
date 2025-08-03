using Microsoft.Data.SqlClient;

using System.Data;

using Domain;

namespace Infrastructure;

public class UsuariosDbContext
{
    private readonly string _connectionString;

    public UsuariosDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<IM253E08Usuario> List()
    {
        var usuarios = new List<IM253E08Usuario>();
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT [Id], [Nombre], [Direccion], [Telefono], [Correo] FROM IM253E08Usuario", connection);
        try
        {
            connection.Open();
            var reader = command.ExecuteReader();
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
        finally
        {
            connection.Close();
        }
    }

    public IM253E08Usuario Details(Guid id)
    {
        var usuario = new IM253E08Usuario();
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT [Id], [Nombre], [Direccion], [Telefono], [Correo] FROM IM253E08Usuario WHERE Id = @Id", connection);
        command.Parameters.Add(new SqlParameter("@Id", id));

        try
        {
            connection.Open();
            var reader = command.ExecuteReader();
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
        finally
        {
            connection.Close();
        }
    }

    public void Create(IM253E08Usuario data)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
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
        finally
        {
            connection.Close();
        }
    }

    public void Edit(IM253E08Usuario data)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
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
        finally
        {
            connection.Close();
        }
    }

    public void Delete(Guid id)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("DELETE FROM IM253E08Usuario WHERE Id = @Id", connection);
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
        finally
        {
            connection.Close();
        }
    }
}
