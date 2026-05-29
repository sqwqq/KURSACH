using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

[Authorize]
public class AdminAthleteController : Controller
{
    private readonly AppDbContext _context;

    public AdminAthleteController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var athletes = await _context.Athletes
            .Include(a => a.Team)
            .OrderBy(a => a.LastName)
            .ToListAsync();
        return View(athletes);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name).ToList(), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Athlete athlete)
    {
        if (athlete.TeamId == 0)
        {
            ModelState.AddModelError("TeamId", "Выберите команду");
        }

        if (ModelState.IsValid)
        {
            _context.Athletes.Add(athlete);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Спортсмен успешно добавлен";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name).ToList(), "Id", "Name");
        return View(athlete);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var athlete = await _context.Athletes.FindAsync(id);
        if (athlete == null)
            return NotFound();

        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name).ToList(), "Id", "Name");
        return View(athlete);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Athlete athlete)
    {
        if (id != athlete.Id)
            return NotFound();

        if (athlete.TeamId == 0)
        {
            ModelState.AddModelError("TeamId", "Выберите команду");
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingAthlete = await _context.Athletes.FindAsync(id);
                if (existingAthlete == null)
                    return NotFound();

                existingAthlete.FirstName = athlete.FirstName;
                existingAthlete.LastName = athlete.LastName;
                existingAthlete.Group = athlete.Group;
                existingAthlete.TeamId = athlete.TeamId;
                existingAthlete.PhotoUrl = athlete.PhotoUrl;
                existingAthlete.Bio = athlete.Bio;
                existingAthlete.Height = athlete.Height;
                existingAthlete.Weight = athlete.Weight;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Спортсмен успешно обновлен";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка сохранения: " + ex.Message);
            }
        }
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name).ToList(), "Id", "Name");
        return View(athlete);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var athlete = await _context.Athletes
            .Include(a => a.Team)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (athlete == null)
            return NotFound();

        return View(athlete);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var athlete = await _context.Athletes.FindAsync(id);
        if (athlete != null)
        {
            _context.Athletes.Remove(athlete);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Спортсмен успешно удален";
        }
        return RedirectToAction(nameof(Index));
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