
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class IncidentsController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public IncidentsController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // GET: INCIDENTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Incidents.ToListAsync());
    }

    // GET: INCIDENTS/Details/5
    public async Task<IActionResult> Details(string? incidentid)
    {
        if (incidentid == null)
        {
            return NotFound();
        }

        var incident = await _context.Incidents
            .FirstOrDefaultAsync(m => m.IncidentId == incidentid);
        if (incident == null)
        {
            return NotFound();
        }

        return View(incident);
    }

    // GET: INCIDENTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: INCIDENTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IncidentId,LineId,StationId,Description,ReportedTime,Severity,Line,Station")] Incident incident)
    {
        if (ModelState.IsValid)
        {
            _context.Add(incident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(incident);
    }

    // GET: INCIDENTS/Edit/5
    public async Task<IActionResult> Edit(string? incidentid)
    {
        if (incidentid == null)
        {
            return NotFound();
        }

        var incident = await _context.Incidents.FindAsync(incidentid);
        if (incident == null)
        {
            return NotFound();
        }
        return View(incident);
    }

    // POST: INCIDENTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? incidentid, [Bind("IncidentId,LineId,StationId,Description,ReportedTime,Severity,Line,Station")] Incident incident)
    {
        if (incidentid != incident.IncidentId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(incident);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(incident.IncidentId))
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
        return View(incident);
    }

    // GET: INCIDENTS/Delete/5
    public async Task<IActionResult> Delete(string? incidentid)
    {
        if (incidentid == null)
        {
            return NotFound();
        }

        var incident = await _context.Incidents
            .FirstOrDefaultAsync(m => m.IncidentId == incidentid);
        if (incident == null)
        {
            return NotFound();
        }

        return View(incident);
    }

    // POST: INCIDENTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? incidentid)
    {
        var incident = await _context.Incidents.FindAsync(incidentid);
        if (incident != null)
        {
            _context.Incidents.Remove(incident);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool IncidentExists(string? incidentid)
    {
        return _context.Incidents.Any(e => e.IncidentId == incidentid);
    }
}
