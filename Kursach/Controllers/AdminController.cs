using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kursach.Controllers;

public class AdminController : Controller
{
    private const string AdminLogin = "admin";
    private const string AdminPassword = "admin123";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
        ViewBag.AdminName = HttpContext.Session.GetString("AdminName");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (HttpContext.Session.GetString("IsAdmin") == "true")
        {
            return RedirectToAction("Index", "Admin");
        }
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string login, string password, string? returnUrl)
    {
        if (login == AdminLogin && password == AdminPassword)
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("AdminName", "Администратор");
            
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Admin");
        }
        
        ViewBag.Error = "Неверный логин или пароль";
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}

public class AuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Session.GetString("IsAdmin") != "true")
        {
            context.Result = new RedirectToActionResult("Login", "Admin", new { returnUrl = context.HttpContext.Request.Path });
        }
        base.OnActionExecuting(context);
    }
}