using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

[Authorize]
public class AdminTeamController : Controller
{
    private readonly AppDbContext _context;

    public AdminTeamController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var teams = await _context.Teams
            .Include(t => t.Athletes)
            .Include(t => t.College)
            .OrderBy(t => t.Name)
            .ToListAsync();
        return View(teams);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Colleges = new SelectList(_context.Colleges.OrderBy(c => c.Name), "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Team team)
    {
        if (ModelState.IsValid)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Команда успешно добавлена";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Colleges = new SelectList(_context.Colleges.OrderBy(c => c.Name), "Id", "Name", team.CollegeId);
        return View(team);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var team = await _context.Teams.FindAsync(id);
        if (team == null) return NotFound();
        ViewBag.Colleges = new SelectList(_context.Colleges.OrderBy(c => c.Name), "Id", "Name", team.CollegeId);
        return View(team);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Team team)
    {
        if (id != team.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var existing = await _context.Teams.FindAsync(id);
                if (existing == null) return NotFound();
                existing.Name = team.Name;
                existing.SportType = team.SportType;
                existing.CoachName = team.CoachName;
                existing.Description = team.Description;
                existing.CollegeId = team.CollegeId;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Команда успешно обновлена";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка сохранения: " + ex.Message);
            }
        }
        ViewBag.Colleges = new SelectList(_context.Colleges.OrderBy(c => c.Name), "Id", "Name", team.CollegeId);
        return View(team);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var team = await _context.Teams
            .Include(t => t.Athletes)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (team == null) return NotFound();
        return View(team);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var team = await _context.Teams
            .Include(t => t.Athletes)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (team != null)
        {
            if (team.Athletes.Any())
            {
                TempData["Error"] = "Нельзя удалить команду, в которой есть спортсмены";
                return RedirectToAction(nameof(Index));
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Команда успешно удалена";
        }
        return RedirectToAction(nameof(Index));
    }
}
