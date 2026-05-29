using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;

namespace Kursach.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var topAthletes = await _context.Athletes
            .Include(a => a.Team)
            .Include(a => a.Achievements)
            .OrderByDescending(a => a.Achievements.Count)
            .ThenByDescending(a => a.Achievements.Count(a => a.Place == "1 место"))
            .Take(5)
            .ToListAsync();

        var latestNews = await _context.News
            .Include(n => n.Athlete)
            .OrderByDescending(n => n.Date)
            .Take(5)
            .ToListAsync();

        var featuredNews = await _context.News
            .Where(n => n.IsFeatured)
            .OrderByDescending(n => n.Date)
            .Take(2)
            .ToListAsync();

        var sportTypes = await _context.Teams
            .Select(t => t.SportType)
            .Distinct()
            .Take(6)
            .ToListAsync();

        var stats = new
        {
            TotalAthletes = await _context.Athletes.CountAsync(),
            TotalAchievements = await _context.Achievements.CountAsync(),
            TotalTeams = await _context.Teams.CountAsync()
        };

        ViewBag.TopAthletes = topAthletes;
        ViewBag.LatestNews = latestNews;
        ViewBag.FeaturedNews = featuredNews;
        ViewBag.SportTypes = new SelectList(sportTypes);
        ViewBag.Stats = stats;

        return View();
    }
}