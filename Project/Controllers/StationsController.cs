using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class StationsController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public StationsController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var station = await _context.Stations
            .OrderBy(s => s.StationId)
            .ToListAsync();

        return View(station);
    }

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
    //CREATE
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
    [Bind("StationId,StationName,Address")] Station station)
    {
        if (await _context.Stations
            .AnyAsync(s => s.StationId == station.StationId))
        {
            ModelState.AddModelError(
                "StationId",
                "Mã ga đã tồn tại.");
        }

        if (ModelState.IsValid)
        {
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(station);
    }
    public async Task<IActionResult> Edit(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var station = await _context.Stations
            .FindAsync(id);

        if (station == null)
        {
            return NotFound();
        }

        return View(station);
    }
    //Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    string id,
    [Bind("StationId,StationName,Address")] Station station)
    {
        if (id != station.StationId)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(station);
        }

        try
        {
            _context.Stations.Update(station);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StationExists(station.StationId))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    //delete
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
    //Delete confirm
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var station = await _context.Stations
            .FindAsync(id);

        if (station == null)
        {
            return NotFound();
        }

        try
        {
            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                "",
                "Không thể xóa ga vì ga đang được sử dụng trong hệ thống.");

            return View("Delete", station);
        }

        return RedirectToAction(nameof(Index));
    }
    //Kiểm tra ID ga Exits or not
    private bool StationExists(string stationId)
    {
        return _context.Stations
            .Any(s => s.StationId == stationId);
    }
}