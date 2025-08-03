using Microsoft.Data.SqlClient;
using System.Data;
using Domain;

namespace Infrastructure;

public class PrestamosDbContext
{
    private readonly string _connectionString;

    public PrestamosDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<IM253E08Prestamo> List()
    {
        var prestamos = new List<IM253E08Prestamo>();
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
            "SELECT Id, UsuarioId, LibroId, FechaPrestamo, FechaDevolucion FROM IM253E08Prestamos",
            connection);

        try
        {
            connection.Open();
            var reader = command.ExecuteReader();
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
        finally
        {
            connection.Close();
        }
    }

    public IM253E08Prestamo Details(Guid id)
    {
        var prestamo = new IM253E08Prestamo();
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
            "SELECT Id, UsuarioId, LibroId, FechaPrestamo, FechaDevolucion FROM IM253E08Prestamos WHERE Id = @Id",
            connection);
        command.Parameters.Add(new SqlParameter("@Id", id));

        try
        {
            connection.Open();
            var reader = command.ExecuteReader();
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
        finally
        {
            connection.Close();
        }
    }

    public void Create(IM253E08Prestamo data)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
            "INSERT INTO IM253E08Prestamos (Id, UsuarioId, LibroId, FechaPrestamo, FechaDevolucion) " +
            "VALUES (@Id, @UsuarioId, @LibroId, @FechaPrestamo, @FechaDevolucion)", connection);

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
        finally
        {
            connection.Close();
        }
    }

    public void Edit(IM253E08Prestamo data)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(
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
        finally
        {
            connection.Close();
        }
    }

    public void Delete(Guid id)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("DELETE FROM IM253E08Prestamos WHERE Id = @Id", connection);
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
        finally
        {
            connection.Close();
        }
    }
}