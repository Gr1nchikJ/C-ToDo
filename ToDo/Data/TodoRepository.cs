using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Serilog;
using ToDo.Models;
using ToDo.Models.ViewModels;

namespace ToDo.Data
{
    public class TodoRepository : ITodoRepository
    {
        public TodoItem GetById(int id)
        {
            TodoItem todo = new();

            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo WHERE Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            todo.Id = reader.GetInt32(0);
                            todo.Name = reader.GetString(1);
                        }
                        else
                        {
                            return todo;
                        }
                    };
                }
            }

            return todo;
        }
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
        public TodoViewModel GetAllTodos()
        {
            List<TodoItem> todoList = new();

            using (SqliteConnection con = new SqliteConnection("Data Source=db.sqlite"))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = "SELECT * FROM todo";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                todoList.Add(
                                    new TodoItem
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1)
                                    });
                            }
                        }
                        else
                        {
                            return new TodoViewModel
                            {
                                TodoList = todoList
                            };
                        }
                    }
                }


            }

            return new TodoViewModel
            {
                TodoList = todoList
            };
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
