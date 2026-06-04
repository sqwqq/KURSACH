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
    private readonly IWebHostEnvironment _env;

    public AdminAthleteController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
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
    public async Task<IActionResult> Create(Athlete athlete, IFormFile? photo, string? photoUrl)
    {
        if (athlete.TeamId == 0)
            ModelState.AddModelError("TeamId", "Выберите команду");

        if (ModelState.IsValid)
        {
            if (photo != null && photo.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsDir);
                var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(stream);
                athlete.PhotoUrl = "/uploads/" + fileName;
            }
            else if (!string.IsNullOrWhiteSpace(photoUrl))
            {
                athlete.PhotoUrl = photoUrl;
            }

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
    public async Task<IActionResult> Edit(int id, Athlete athlete, IFormFile? photo, string? photoUrl)
    {
        if (id != athlete.Id)
            return NotFound();

        if (athlete.TeamId == 0)
            ModelState.AddModelError("TeamId", "Выберите команду");

        if (ModelState.IsValid)
        {
            var existingAthlete = await _context.Athletes.FindAsync(id);
            if (existingAthlete == null)
                return NotFound();

            if (photo != null && photo.Length > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsDir);
                var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(stream);
                existingAthlete.PhotoUrl = "/uploads/" + fileName;
            }
            else if (!string.IsNullOrWhiteSpace(photoUrl))
            {
                existingAthlete.PhotoUrl = photoUrl;
            }

            existingAthlete.FirstName = athlete.FirstName;
            existingAthlete.LastName = athlete.LastName;
            existingAthlete.Group = athlete.Group;
            existingAthlete.TeamId = athlete.TeamId;
            existingAthlete.Bio = athlete.Bio;
            existingAthlete.Height = athlete.Height;
            existingAthlete.Weight = athlete.Weight;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Спортсмен успешно обновлен";
            return RedirectToAction(nameof(Index));
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