using Microsoft.AspNetCore.Mvc;

namespace ServiceLog.Controllers;

public class VehicleServiceController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
