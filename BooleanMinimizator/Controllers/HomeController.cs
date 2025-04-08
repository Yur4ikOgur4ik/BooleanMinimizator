using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BooleanMinimizator.Models;
using BooleanMinimizerLibrary;

namespace BooleanMinimizator.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new BooleanMinimizatorModel());
    }

    [HttpPost]
    public IActionResult Index(BooleanMinimizatorModel model)
    {
        var parser = new SyntaxAnalyzer();

        try
        {
            parser.Parse(model.InputFunction);
            model.ResultMessage = "Функция успешно распознана!";
        }
        catch (ArgumentException ex)
        {
            model.ResultMessage = ex.Message;
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
