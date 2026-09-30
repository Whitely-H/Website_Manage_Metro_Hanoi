using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Microsoft.AspNetCore.Authorization;

public class SchedulesController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public SchedulesController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // Details
    [Authorize]
    public async Task<IActionResult> Details(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var schedule = await _context.Schedules
            .Include(s => s.Train)
            .FirstOrDefaultAsync(s => s.ScheduleId == id);

        if (schedule == null)
        {
            return NotFound();
        }

        return View(schedule);
    }

    // Create
    [Authorize(Roles = "Staff,Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("ScheduleId,TrainId,DepartureTime,ArrivalTime,Direction")]
        Schedule schedule)
    {
        if (ModelState.IsValid)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(schedule);
    }

    // Edit
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Edit(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var schedule = await _context.Schedules
            .FindAsync(id);

        if (schedule == null)
        {
            return NotFound();
        }

        return View(schedule);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        string id,
        [Bind("ScheduleId,TrainId,DepartureTime,ArrivalTime,Direction")]
        Schedule schedule)
    {
        if (id != schedule.ScheduleId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Schedules.Update(schedule);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(schedule.ScheduleId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(schedule);
    }

    // Delete
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Delete(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var schedule = await _context.Schedules
            .FirstOrDefaultAsync(s => s.ScheduleId == id);

        if (schedule == null)
        {
            return NotFound();
        }

        return View(schedule);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var schedule = await _context.Schedules
            .FindAsync(id);

        if (schedule == null)
        {
            return NotFound();
        }

        _context.Schedules.Remove(schedule);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // Index
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var schedules = await _context.Schedules
            .Include(s => s.Train)
            .OrderBy(s => s.DepartureTime)
            .ToListAsync();

        return View(schedules);
    }

    private bool ScheduleExists(string id)
    {
        return _context.Schedules
            .Any(s => s.ScheduleId == id);
    }
}