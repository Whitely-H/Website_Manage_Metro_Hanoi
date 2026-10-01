using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers;

public class HomeController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public HomeController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? q)
    {
        if (!string.IsNullOrWhiteSpace(q))
        {
            ViewBag.SearchLines = await _context.Lines
                .Where(x => x.LineID.Contains(q) || x.LineName.Contains(q))
                .ToListAsync();

            ViewBag.SearchStations = await _context.Stations
                .Where(x => x.StationId.Contains(q) || x.StationName.Contains(q))
                .ToListAsync();

            ViewBag.Search = q;
        }

        return View();
    }
}