using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

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
        var upcomingCompetitions = await _context.Competitions
            .Include(c => c.Team)
            .Where(c => !c.IsCompleted && c.Date >= DateTime.Today)
            .OrderBy(c => c.Date)
            .Take(3)
            .ToListAsync();

        ViewBag.SportTypes = new SelectList(sportTypes);
        ViewBag.Stats = stats;
        ViewBag.UpcomingCompetitions = upcomingCompetitions;

        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SendContact(string name, string email, string subject, string message)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(message))
        {
            TempData["Success"] = "Заполните все поля";
            return RedirectToAction("Contact");
        }

        TempData["Success"] = "Спасибо! Ваше сообщение отправлено. Мы ответим вам в ближайшее время.";
        return RedirectToAction("Contact");
    }

    public async Task<IActionResult> Leaderboard(string? sportType)
    {
        var query = _context.Athletes
            .Include(a => a.Team)
            .Include(a => a.Achievements)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(sportType))
        {
            query = query.Where(a => a.Team != null && a.Team.SportType == sportType);
        }

        var athletes = await query
            .OrderByDescending(a => a.Achievements.Count)
            .ThenByDescending(a => a.Achievements.Count(a => a.Place != null && a.Place.StartsWith("1 ")))
            .ToListAsync();

        var sportTypes = await _context.Teams
            .Select(t => t.SportType)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync();

        ViewBag.SportTypes = new SelectList(sportTypes, sportType);
        ViewBag.SelectedSport = sportType;
        return View(athletes);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return View(new SearchResult());
        }

        var lowerQ = q.ToLower();

        var athletes = await _context.Athletes
            .Include(a => a.Team)
            .Where(a => a.FirstName.ToLower().Contains(lowerQ) ||
                       a.LastName.ToLower().Contains(lowerQ) ||
                       a.Group.ToLower().Contains(lowerQ))
            .Take(5)
            .ToListAsync();

        var news = await _context.News
            .Where(n => n.Title.ToLower().Contains(lowerQ) ||
                       n.Content.ToLower().Contains(lowerQ))
            .Take(5)
            .ToListAsync();

        var achievements = await _context.Achievements
            .Include(a => a.Athlete)
            .Where(a => a.Title.ToLower().Contains(lowerQ) ||
                       a.SportType.ToLower().Contains(lowerQ))
            .Take(5)
            .ToListAsync();

        var teams = await _context.Teams
            .Where(t => t.Name.ToLower().Contains(lowerQ) ||
                       t.SportType.ToLower().Contains(lowerQ))
            .Take(5)
            .ToListAsync();

        var result = new SearchResult
        {
            Query = q,
            Athletes = athletes,
            News = news,
            Achievements = achievements,
            Teams = teams
        };

        return View(result);
    }
}