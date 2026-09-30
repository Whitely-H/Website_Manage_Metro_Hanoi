using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers;

public class StationsController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public StationsController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Stations.ToListAsync());
    }

    [Authorize]
    public async Task<IActionResult> Details(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var station = await _context.Stations
            .FirstOrDefaultAsync(s => s.StationId == id);

        if (station == null)
        {
            return NotFound();
        }

        return View(station);
    }

    // Staff + Admin
    [Authorize(Roles = "Staff,Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Create(
        [Bind("StationId,StationName,Address")]
        Station station)
    {
        if (ModelState.IsValid)
        {
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(station);
    }

    // Staff + Admin
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Edit(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var station = await _context.Stations.FindAsync(id);

        if (station == null)
        {
            return NotFound();
        }

        return View(station);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Edit(
        string id,
        [Bind("StationId,StationName,Address")]
        Station station)
    {
        if (id != station.StationId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Stations.Update(station);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(station);
    }

    // Staff + Admin
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Delete(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var station = await _context.Stations
            .FirstOrDefaultAsync(s => s.StationId == id);

        if (station == null)
        {
            return NotFound();
        }

        return View(station);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var station = await _context.Stations.FindAsync(id);

        if (station == null)
        {
            return NotFound();
        }

        _context.Stations.Remove(station);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}