using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

[Authorize]
public class AdminNewsController : Controller
{
    private readonly AppDbContext _context;

    public AdminNewsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var news = await _context.News
            .Include(n => n.Athlete)
            .OrderByDescending(n => n.Date)
            .ToListAsync();
        return View(news);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Athletes = new SelectList(_context.Athletes.OrderBy(a => a.LastName), "Id", "LastName");
        ViewBag.Categories = new SelectList(new[] { "Спорт", "Новости", "События" });
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(News news)
    {
        if (ModelState.IsValid)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Новость успешно добавлена";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Athletes = new SelectList(_context.Athletes.OrderBy(a => a.LastName), "Id", "LastName", news.AthleteId);
        ViewBag.Categories = new SelectList(new[] { "Спорт", "Новости", "События" });
        return View(news);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var news = await _context.News.FindAsync(id);
        if (news == null)
            return NotFound();

        ViewBag.Athletes = new SelectList(_context.Athletes.OrderBy(a => a.LastName), "Id", "LastName", news.AthleteId);
        ViewBag.Categories = new SelectList(new[] { "Спорт", "Новости", "События" });
        return View(news);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, News news)
    {
        if (id != news.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existingNews = await _context.News.FindAsync(id);
                if (existingNews == null)
                    return NotFound();

                existingNews.Title = news.Title;
                existingNews.Content = news.Content;
                existingNews.Date = news.Date;
                existingNews.Category = news.Category;
                existingNews.AthleteId = news.AthleteId;
                existingNews.ImageUrl = news.ImageUrl;

                await _context.SaveChangesAsync();
                TempData["Success"] = "Новость успешно обновлена";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка сохранения: " + ex.Message);
            }
        }
        ViewBag.Athletes = new SelectList(_context.Athletes.OrderBy(a => a.LastName), "Id", "LastName", news.AthleteId);
        ViewBag.Categories = new SelectList(new[] { "Спорт", "Новости", "События" });
        return View(news);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var news = await _context.News
            .Include(n => n.Athlete)
            .FirstOrDefaultAsync(n => n.Id == id);
        
        if (news == null)
            return NotFound();

        return View(news);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var news = await _context.News.FindAsync(id);
        if (news != null)
        {
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Новость успешно удалена";
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var news = await _context.News
            .Include(n => n.Athlete)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (news == null)
            return NotFound();

        return View(news);
    }
}