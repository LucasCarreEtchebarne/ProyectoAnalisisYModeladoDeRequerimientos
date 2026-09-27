using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemaHotelColibri.Models;

namespace SistemaHotelColibri.Controllers;

public class HomeController : ControladorBase
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
