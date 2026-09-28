
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class TicketTypesController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public TicketTypesController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // GET: TICKETTYPES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TicketTypes.ToListAsync());
    }

    // GET: TICKETTYPES/Details/5
    public async Task<IActionResult> Details(string? tickettypeid)
    {
        if (tickettypeid == null)
        {
            return NotFound();
        }

        var tickettype = await _context.TicketTypes
            .FirstOrDefaultAsync(m => m.TicketTypeId == tickettypeid);
        if (tickettype == null)
        {
            return NotFound();
        }

        return View(tickettype);
    }

    // GET: TICKETTYPES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TICKETTYPES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TicketTypeId,TypeName,Price,DurationHours,Orders")] TicketType tickettype)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tickettype);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tickettype);
    }

    // GET: TICKETTYPES/Edit/5
    public async Task<IActionResult> Edit(string? tickettypeid)
    {
        if (tickettypeid == null)
        {
            return NotFound();
        }

        var tickettype = await _context.TicketTypes.FindAsync(tickettypeid);
        if (tickettype == null)
        {
            return NotFound();
        }
        return View(tickettype);
    }

    // POST: TICKETTYPES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? tickettypeid, [Bind("TicketTypeId,TypeName,Price,DurationHours,Orders")] TicketType tickettype)
    {
        if (tickettypeid != tickettype.TicketTypeId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tickettype);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketTypeExists(tickettype.TicketTypeId))
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
        return View(tickettype);
    }

    // GET: TICKETTYPES/Delete/5
    public async Task<IActionResult> Delete(string? tickettypeid)
    {
        if (tickettypeid == null)
        {
            return NotFound();
        }

        var tickettype = await _context.TicketTypes
            .FirstOrDefaultAsync(m => m.TicketTypeId == tickettypeid);
        if (tickettype == null)
        {
            return NotFound();
        }

        return View(tickettype);
    }

    // POST: TICKETTYPES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? tickettypeid)
    {
        var tickettype = await _context.TicketTypes.FindAsync(tickettypeid);
        if (tickettype != null)
        {
            _context.TicketTypes.Remove(tickettype);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TicketTypeExists(string? tickettypeid)
    {
        return _context.TicketTypes.Any(e => e.TicketTypeId == tickettypeid);
    }
}
