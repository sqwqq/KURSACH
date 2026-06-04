using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

[Authorize]
public class AdminCollegeController : Controller
{
    private readonly AppDbContext _context;

    public AdminCollegeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var colleges = await _context.Colleges
            .Include(c => c.Teams)
            .OrderBy(c => c.Name)
            .ToListAsync();
        return View(colleges);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(College college)
    {
        if (ModelState.IsValid)
        {
            _context.Colleges.Add(college);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Колледж успешно добавлен";
            return RedirectToAction(nameof(Index));
        }
        return View(college);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var college = await _context.Colleges.FindAsync(id);
        if (college == null) return NotFound();
        return View(college);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, College college)
    {
        if (id != college.Id) return NotFound();
        if (ModelState.IsValid)
        {
            try
            {
                var existing = await _context.Colleges.FindAsync(id);
                if (existing == null) return NotFound();
                existing.Name = college.Name;
                existing.City = college.City;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Колледж успешно обновлён";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка сохранения: " + ex.Message);
            }
        }
        return View(college);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var college = await _context.Colleges
            .Include(c => c.Teams)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (college == null) return NotFound();
        return View(college);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var college = await _context.Colleges
            .Include(c => c.Teams)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (college != null)
        {
            if (college.Teams.Any())
            {
                TempData["Error"] = "Нельзя удалить колледж, в котором есть команды";
                return RedirectToAction(nameof(Index));
            }
            _context.Colleges.Remove(college);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Колледж успешно удалён";
        }
        return RedirectToAction(nameof(Index));
    }
}
