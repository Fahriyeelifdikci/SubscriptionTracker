
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SubscriptionTracker.Web.Models;

namespace SubscriptionTracker.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Dashboard");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error(int? statusCode = null)
    {
        if (statusCode.HasValue && statusCode.Value >= 400)
        {
            Response.StatusCode = statusCode.Value;
        }
        else
        {
            Response.StatusCode = 500;
        }

        ViewData["StatusCode"] = Response.StatusCode;

        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
        });
    }
}
