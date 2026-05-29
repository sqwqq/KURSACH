using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

public class AchievementController : Controller
{
    private readonly AppDbContext _context;

    public AchievementController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string sportType, int? athleteId)
    {
        var sportTypes = await _context.Achievements
            .Select(a => a.SportType)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync();

        var athletes = await _context.Athletes
            .OrderBy(a => a.LastName)
            .ToListAsync();

        ViewBag.SportTypes = new SelectList(sportTypes);
        ViewBag.Athletes = new SelectList(athletes, "Id", "LastName");
        ViewBag.SelectedSportType = sportType;
        ViewBag.SelectedAthleteId = athleteId;

        var achievements = _context.Achievements
            .Include(a => a.Athlete)
            .ThenInclude(at => at!.Team)
            .ThenInclude(t => t!.College)
            .AsQueryable();

        if (!string.IsNullOrEmpty(sportType))
        {
            achievements = achievements.Where(a => a.SportType == sportType);
        }

        if (athleteId.HasValue)
        {
            achievements = achievements.Where(a => a.AthleteId == athleteId.Value);
        }

        var result = await achievements
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        return View(result);
    }
}