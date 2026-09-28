
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class PassengersController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public PassengersController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // GET: PASSENGERS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Passengers.ToListAsync());
    }

    // GET: PASSENGERS/Details/5
    public async Task<IActionResult> Details(System.Guid? passengerid)
    {
        if (passengerid == null)
        {
            return NotFound();
        }

        var passenger = await _context.Passengers
            .FirstOrDefaultAsync(m => m.PassengerId == passengerid);
        if (passenger == null)
        {
            return NotFound();
        }

        return View(passenger);
    }

    // GET: PASSENGERS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PASSENGERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PassengerId,FullName,PhoneNumber,Email,IdentityCard,PassengerType,Orders,SmartCards")] Passenger passenger)
    {
        if (ModelState.IsValid)
        {
            _context.Add(passenger);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(passenger);
    }

    // GET: PASSENGERS/Edit/5
    public async Task<IActionResult> Edit(System.Guid? passengerid)
    {
        if (passengerid == null)
        {
            return NotFound();
        }

        var passenger = await _context.Passengers.FindAsync(passengerid);
        if (passenger == null)
        {
            return NotFound();
        }
        return View(passenger);
    }

    // POST: PASSENGERS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(System.Guid? passengerid, [Bind("PassengerId,FullName,PhoneNumber,Email,IdentityCard,PassengerType,Orders,SmartCards")] Passenger passenger)
    {
        if (passengerid != passenger.PassengerId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(passenger);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(passenger.PassengerId))
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
        return View(passenger);
    }

    // GET: PASSENGERS/Delete/5
    public async Task<IActionResult> Delete(System.Guid? passengerid)
    {
        if (passengerid == null)
        {
            return NotFound();
        }

        var passenger = await _context.Passengers
            .FirstOrDefaultAsync(m => m.PassengerId == passengerid);
        if (passenger == null)
        {
            return NotFound();
        }

        return View(passenger);
    }

    // POST: PASSENGERS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(System.Guid? passengerid)
    {
        var passenger = await _context.Passengers.FindAsync(passengerid);
        if (passenger != null)
        {
            _context.Passengers.Remove(passenger);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PassengerExists(System.Guid? passengerid)
    {
        return _context.Passengers.Any(e => e.PassengerId == passengerid);
    }
}
