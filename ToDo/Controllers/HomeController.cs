using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using Serilog.Events;
using ToDo.Models;
using ToDo.Models.ViewModels;
using ToDo.Data;

namespace ToDo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITodoRepository _todoRepository;

    public HomeController(ILogger<HomeController> logger, ITodoRepository todoRepository)
    {
        _logger = logger;
        _todoRepository = todoRepository;
    }
    public IActionResult Index()
    {
        var todoListViewModel = GetAllTodos();

        

        
        try
        {
            Log.Information("Application Starting Up");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application failed to start correctly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
        return View(todoListViewModel);

    }

    public JsonResult PopulateForm(int id)
    {
        var todo = GetById(id);
        return Json(todo);
    }

    internal TodoViewModel GetAllTodos()
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

    internal TodoItem GetById(int id)
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

    public RedirectResult Insert(TodoItem todo)
    {
        _todoRepository.Insert(todo);
        return Redirect("https://localhost:7161/");
    }

    public RedirectResult Update(TodoItem todo)
    {
        _todoRepository.Update(todo);
        return Redirect("https://localhost:7161/");
    }

    public JsonResult Delete(int id)
    {
        _todoRepository.Delete(id);
        return Json(new { });
    }
}
