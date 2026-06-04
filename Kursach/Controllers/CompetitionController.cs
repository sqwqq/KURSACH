using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

public class CompetitionController : Controller
{
    private readonly AppDbContext _context;

    public CompetitionController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var upcoming = await _context.Competitions
            .Include(c => c.Team)
            .Where(c => !c.IsCompleted && c.Date >= DateTime.Today)
            .OrderBy(c => c.Date)
            .ToListAsync();

        var completed = await _context.Competitions
            .Include(c => c.Team)
            .Where(c => c.IsCompleted || c.Date < DateTime.Today)
            .OrderByDescending(c => c.Date)
            .Take(10)
            .ToListAsync();

        ViewBag.Upcoming = upcoming;
        ViewBag.Completed = completed;
        return View();
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var competition = await _context.Competitions
            .Include(c => c.Team)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (competition == null) return NotFound();
        return View(competition);
    }
}
