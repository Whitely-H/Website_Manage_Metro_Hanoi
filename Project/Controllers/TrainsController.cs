
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class TrainsController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public TrainsController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // GET: TRAINS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Trains.ToListAsync());
    }

    // GET: TRAINS/Details/5
    public async Task<IActionResult> Details(string? trainid)
    {
        if (trainid == null)
        {
            return NotFound();
        }

        var train = await _context.Trains
            .FirstOrDefaultAsync(m => m.TrainId == trainid);
        if (train == null)
        {
            return NotFound();
        }

        return View(train);
    }

    // GET: TRAINS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TRAINS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TrainId,TrainCode,LineId,Capacity,Status,Line,Schedules")] Train train)
    {
        if (ModelState.IsValid)
        {
            _context.Add(train);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(train);
    }

    // GET: TRAINS/Edit/5
    public async Task<IActionResult> Edit(string? trainid)
    {
        if (trainid == null)
        {
            return NotFound();
        }

        var train = await _context.Trains.FindAsync(trainid);
        if (train == null)
        {
            return NotFound();
        }
        return View(train);
    }

    // POST: TRAINS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? trainid, [Bind("TrainId,TrainCode,LineId,Capacity,Status,Line,Schedules")] Train train)
    {
        if (trainid != train.TrainId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(train);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainExists(train.TrainId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(train);
    }

    // GET: TRAINS/Delete/5
    public async Task<IActionResult> Delete(string? trainid)
    {
        if (trainid == null)
        {
            return NotFound();
        }

        var train = await _context.Trains
            .FirstOrDefaultAsync(m => m.TrainId == trainid);
        if (train == null)
        {
            return NotFound();
        }

        return View(train);
    }

    // POST: TRAINS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? trainid)
    {
        var train = await _context.Trains.FindAsync(trainid);
        if (train != null)
        {
            _context.Trains.Remove(train);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TrainExists(string? trainid)
    {
        return _context.Trains.Any(e => e.TrainId == trainid);
    }
}
