using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using Serilog.Events;
using ToDo.Models;
using ToDo.Models.ViewModels;
using ToDo.Data;
using ToDo.Captcha;

namespace ToDo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITodoRepository _todoRepository;
    private readonly ICaptchaValidator _captchaValidator;

    public HomeController(ILogger<HomeController> logger, ITodoRepository todoRepository, ICaptchaValidator captchaValidator)
    {
        _logger = logger;
        _todoRepository = todoRepository;
        _captchaValidator = captchaValidator;
    }
    public IActionResult Index()
    {
        var todoListViewModel = _todoRepository.GetAllTodos();

        Log.Information("Application Starting Up");
        
        return View(todoListViewModel);

    }
    [HttpGet]
    public IActionResult Captcha()
    {
        Log.Information("Captcha Starting Up");

        return View();

    }
    [HttpPost]
    public async Task<IActionResult> CaptchaPost(CaptchaViewModel model)
    {
        Log.Information("Captcha Starting Up");
        if (!await _captchaValidator.IsCaptchaPassedAsync(model.CaptchaToken))
        {
            ModelState.AddModelError("", "Captcha validation failed");
            return View("Captcha");
        }
        return Redirect("https://localhost:7161/");

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
