using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

public class AthleteController : Controller
{
    private readonly AppDbContext _context;

    public AthleteController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString, string sportType, string sortOrder, int page = 1)
    {
        const int pageSize = 12;

        var sportTypes = await _context.Teams
            .Select(t => t.SportType)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync();

        ViewBag.SportTypes = new SelectList(sportTypes);
        ViewBag.CurrentFilter = searchString;
        ViewBag.CurrentSportType = sportType;
        ViewBag.CurrentSort = sortOrder;
        ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewBag.AchievementsSort = sortOrder == "achievements" ? "achievements_desc" : "achievements";
        ViewBag.Page = page;

        var athletes = _context.Athletes
            .Include(a => a.Team)
            .ThenInclude(t => t!.College)
            .Include(a => a.Achievements)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            athletes = athletes.Where(a => 
                a.FirstName.Contains(searchString) || 
                a.LastName.Contains(searchString));
        }

        if (!string.IsNullOrEmpty(sportType))
        {
            athletes = athletes.Where(a => a.Team!.SportType == sportType);
        }

        athletes = sortOrder switch
        {
            "name_desc" => athletes.OrderByDescending(a => a.LastName),
            "achievements" => athletes.OrderByDescending(a => a.Achievements.Count),
            "achievements_desc" => athletes.OrderBy(a => a.Achievements.Count),
            _ => athletes.OrderBy(a => a.LastName)
        };

        var total = await athletes.CountAsync();
        ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);

        var result = await athletes
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return View(result);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var athlete = await _context.Athletes
            .Include(a => a.Team)
            .ThenInclude(t => t!.College)
            .Include(a => a.Achievements)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (athlete == null)
            return NotFound();

        return View(athlete);
    }
}