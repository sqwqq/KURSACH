using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;

namespace Kursach.Controllers;

public class AdminController : Controller
{
    private readonly string _adminLogin;
    private readonly string _adminPasswordHash;
    private readonly AppDbContext _context;

    public AdminController(IConfiguration config, AppDbContext context)
    {
        _adminLogin = config["Admin:Login"] ?? "admin";
        _adminPasswordHash = config["Admin:PasswordHash"] ?? "";
        _context = context;
    }

    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

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
        if (login == _adminLogin && HashPassword(password) == _adminPasswordHash)
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("AdminName", "Администратор");
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
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
    public async Task<IActionResult> Index()
    {
        ViewBag.TotalAthletes = await _context.Athletes.CountAsync();
        ViewBag.TotalAchievements = await _context.Achievements.CountAsync();
        ViewBag.TotalTeams = await _context.Teams.CountAsync();
        ViewBag.TotalNews = await _context.News.CountAsync();
        ViewBag.TotalColleges = await _context.Colleges.CountAsync();
        ViewBag.TotalGold = await _context.Achievements
            .Where(a => a.Place != null && a.Place.StartsWith("1 "))
            .CountAsync();
        ViewBag.TotalCompetitions = await _context.Competitions.CountAsync();
        ViewBag.UpcomingCompetitions = await _context.Competitions
            .CountAsync(c => !c.IsCompleted && c.Date >= DateTime.Today);

        var sportTypeData = await _context.Achievements
            .GroupBy(a => a.SportType)
            .Select(g => new { SportType = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync();
        ViewBag.ChartLabels = sportTypeData.Select(x => x.SportType).ToList();
        ViewBag.ChartData = sportTypeData.Select(x => x.Count).ToList();

        var monthlyData = await _context.Achievements
            .GroupBy(a => new { a.Date.Year, a.Date.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync();
        ViewBag.MonthlyLabels = monthlyData.Select(x => $"{x.Month:00}.{x.Year}").ToList();
        ViewBag.MonthlyData = monthlyData.Select(x => x.Count).ToList();

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