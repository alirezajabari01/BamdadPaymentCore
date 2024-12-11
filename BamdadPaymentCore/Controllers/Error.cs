using Microsoft.AspNetCore.Mvc;

namespace BamdadPaymentCore.Controllers;

public class Error : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
