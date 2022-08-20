

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Serilog;
using ToDo.Models;

namespace ToDo.Data
{
    public class TodoRepository
    {
        public void Insert(TodoItem todo)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = $"INSERT INTO todo (name) VALUES ('{todo.Name}')";
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Cannot insert value");
                    }
                }
            }
            
        }

        public void Update(TodoItem todo)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}'";
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Cannot update value");
                    }
                }
            }
            
        }

        public void Delete(int id)
        {
            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = $"DELETE from todo WHERE Id = '{id}'";
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Cannot delete value");
                    }
                }
            }
            
        }
    }
}
