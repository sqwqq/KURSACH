using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

[Authorize]
public class AdminCompetitionController : Controller
{
    private readonly AppDbContext _context;

    public AdminCompetitionController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var competitions = await _context.Competitions
            .Include(c => c.Team)
            .OrderByDescending(c => c.Date)
            .ToListAsync();
        return View(competitions);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Competition competition)
    {
        if (ModelState.IsValid)
        {
            _context.Competitions.Add(competition);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Соревнование добавлено";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", competition.TeamId);
        return View(competition);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var comp = await _context.Competitions.FindAsync(id);
        if (comp == null) return NotFound();
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", comp.TeamId);
        return View(comp);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Competition competition)
    {
        if (id != competition.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var existing = await _context.Competitions.FindAsync(id);
                if (existing == null) return NotFound();
                existing.Title = competition.Title;
                existing.Date = competition.Date;
                existing.Location = competition.Location;
                existing.SportType = competition.SportType;
                existing.Description = competition.Description;
                existing.IsCompleted = competition.IsCompleted;
                existing.TeamId = competition.TeamId;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Соревнование обновлено";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка: " + ex.Message);
            }
        }
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", competition.TeamId);
        return View(competition);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var comp = await _context.Competitions.FindAsync(id);
        if (comp == null) return NotFound();
        return View(comp);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var comp = await _context.Competitions.FindAsync(id);
        if (comp != null)
        {
            _context.Competitions.Remove(comp);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Соревнование удалено";
        }
        return RedirectToAction(nameof(Index));
    }
}
