using Microsoft.Data.SqlClient;
using System.Data;

using Domain;

namespace Infrastructure;


public class LibrosDbContext
{
    private readonly string _connectionString;

    public LibrosDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<IM253E08Libro> List()
    {
        var libros = new List<IM253E08Libro>();
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT [Id],[Autor],[Editorial],[ISBN],[Foto] FROM IM253E08Libro", connection);
        try
        {
            connection.Open();
            var reader = command.ExecuteReader();
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
        finally
        {
            connection.Close();
        }
    }

    public IM253E08Libro Details(Guid id)
    {
        var libro = new IM253E08Libro();
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("SELECT [Id],[Autor],[Editorial],[ISBN],[Foto] FROM IM253E08Libro WHERE Id = @Id", connection);
        command.Parameters.Add(new SqlParameter("@Id", id));

        try
        {
            connection.Open();
            var reader = command.ExecuteReader();
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
        finally
        {
            connection.Close();
        }
    }

    public void Create(IM253E08Libro data)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("INSERT INTO IM253E08Libro (Id, Autor, Editorial, ISBN, Foto) VALUES (@Id, @Autor, @Editorial, @ISBN, @Foto)", connection);

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
        finally
        {
            connection.Close();
        }
    }

    public void Edit(IM253E08Libro data)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("UPDATE IM253E08Libro SET Autor = @Autor, Editorial = @Editorial, ISBN = @ISBN, Foto = @Foto WHERE Id = @Id", connection);

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
        finally
        {
            connection.Close();
        }
    }

    public void Delete(Guid id)
    {
        var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand("DELETE FROM IM253E08Libro WHERE Id = @Id", connection);
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
        finally
        {
            connection.Close();
        }
    }
}