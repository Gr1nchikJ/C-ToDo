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
        var todo = _todoRepository.GetById(id);
        return Json(todo);
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
