using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kursach.Data;
using Kursach.Models;

namespace Kursach.Controllers;

public class NewsController : Controller
{
    private readonly AppDbContext _context;

    public NewsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string category)
    {
        var categories = await _context.News
            .Select(n => n.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        ViewBag.Categories = new SelectList(categories);
        ViewBag.SelectedCategory = category;

        var news = _context.News
            .Include(n => n.Athlete)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category))
        {
            news = news.Where(n => n.Category == category);
        }

        var result = await news
            .OrderByDescending(n => n.Date)
            .ToListAsync();

        return View(result);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var newsItem = await _context.News
            .Include(n => n.Athlete)
            .ThenInclude(a => a!.Team)
            .ThenInclude(t => t!.College)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (newsItem == null)
            return NotFound();

        return View(newsItem);
    }
}