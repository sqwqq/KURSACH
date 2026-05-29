using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

[Authorize]
public class AdminAchievementController : Controller
{
    private readonly AppDbContext _context;

    public AdminAchievementController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var achievements = await _context.Achievements
            .Include(a => a.Athlete)
            .OrderByDescending(a => a.Date)
            .ToListAsync();
        return View(achievements);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Athletes = new SelectList(_context.Athletes.OrderBy(a => a.LastName), "Id", "LastName");
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name");
        ViewBag.SportTypes = new SelectList(new[] { "Баскетбол", "Волейбол", "Плавание", "Легкая атлетика", "Теннис", "Футбол" });
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Achievement achievement)
    {
        if (ModelState.IsValid)
        {
            _context.Achievements.Add(achievement);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Достижение успешно добавлено";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Athletes = new SelectList(_context.Athletes.OrderBy(a => a.LastName), "Id", "LastName", achievement.AthleteId);
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", achievement.TeamId);
        ViewBag.SportTypes = new SelectList(new[] { "Баскетбол", "Волейбол", "Плавание", "Легкая атлетика", "Теннис", "Футбол" });
        return View(achievement);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var achievement = await _context.Achievements.FindAsync(id);
        if (achievement == null)
            return NotFound();

        ViewBag.Athletes = new SelectList(_context.Athletes.OrderBy(a => a.LastName), "Id", "LastName", achievement.AthleteId);
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", achievement.TeamId);
        ViewBag.SportTypes = new SelectList(new[] { "Баскетбол", "Волейбол", "Плавание", "Легкая атлетика", "Теннис", "Футбол" });
        return View(achievement);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Achievement achievement)
    {
        if (id != achievement.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existingAchievement = await _context.Achievements.FindAsync(id);
                if (existingAchievement == null)
                    return NotFound();

                existingAchievement.Title = achievement.Title;
                existingAchievement.Date = achievement.Date;
                existingAchievement.SportType = achievement.SportType;
                existingAchievement.AthleteId = achievement.AthleteId;
                existingAchievement.TeamId = achievement.TeamId;
                existingAchievement.Place = achievement.Place;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Достижение успешно обновлено";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка сохранения: " + ex.Message);
            }
        }
        ViewBag.Athletes = new SelectList(_context.Athletes.OrderBy(a => a.LastName), "Id", "LastName", achievement.AthleteId);
        ViewBag.Teams = new SelectList(_context.Teams.OrderBy(t => t.Name), "Id", "Name", achievement.TeamId);
        ViewBag.SportTypes = new SelectList(new[] { "Баскетбол", "Волейбол", "Плавание", "Легкая атлетика", "Теннис", "Футбол" });
        return View(achievement);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var achievement = await _context.Achievements
            .Include(a => a.Athlete)
            .FirstOrDefaultAsync(a => a.Id == id);
        
        if (achievement == null)
            return NotFound();

        return View(achievement);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var achievement = await _context.Achievements.FindAsync(id);
        if (achievement != null)
        {
            _context.Achievements.Remove(achievement);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Достижение успешно удалено";
        }
        return RedirectToAction(nameof(Index));
    }
}