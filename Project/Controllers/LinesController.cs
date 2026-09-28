using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class LinesController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public LinesController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // GET: Lines
    public async Task<IActionResult> Index()
    {
        var lines = await _context.Lines
            .Include(l => l.Incidents)
            .Include(l => l.Trains)
            .ToListAsync();

        return View(lines);
    }

    public async Task<IActionResult> Details(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var line = await _context.Lines
            .Include(l => l.Trains)
            .Include(l => l.Incidents)
            .Include(l => l.LineStations)
            .ThenInclude(ls => ls.Station)
            .FirstOrDefaultAsync(l => l.LineID == id);

        if (line == null)
        {
            return NotFound();
        }

        return View(line);
    }

    // GET: Lines/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Lines/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
    [Bind("LineID,LineName,ColorCode,Status")] Line line)
    {
        if (ModelState.IsValid)
        {
            _context.Lines.Add(line);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(line);
    }

    // GET: Lines/Edit/L-2A
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var line = await _context.Lines.FindAsync(id);

        if (line == null)
        {
            return NotFound();
        }

        return View(line);
    }

    // POST: Lines/Edit/L-2A
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        string id,
        [Bind("LineID,LineName,ColorCode,Status")] Line line)
    {
        if (id != line.LineID)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(line);
        }

        try
        {
            _context.Lines.Update(line);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LineExists(line.LineID))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Lines/Delete/L-2A
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var line = await _context.Lines
            .FirstOrDefaultAsync(m => m.LineID == id);

        if (line == null)
        {
            return NotFound();
        }

        return View(line);
    }

    // POST: Lines/Delete/L-2A
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var line = await _context.Lines.FindAsync(id);

        if (line == null)
        {
            return NotFound();
        }

        try
        {
            _context.Lines.Remove(line);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                "",
                "Không thể xóa tuyến vì tuyến đang được sử dụng bởi dữ liệu khác.");

            return View("Delete", line);
        }

        return RedirectToAction(nameof(Index));
    }

    private bool LineExists(string id)
    {
        return _context.Lines.Any(e => e.LineID == id);
    }
}
