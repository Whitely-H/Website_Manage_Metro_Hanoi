using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers;

[Authorize]
public class PassengersController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public PassengersController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // Index
    public async Task<IActionResult> Index()
    {
        return View(await _context.Passengers.ToListAsync());
    }

    // Details
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var passenger = await _context.Passengers
            .FirstOrDefaultAsync(p => p.PassengerId == id);

        if (passenger == null)
        {
            return NotFound();
        }

        return View(passenger);
    }

    // Create
    [Authorize(Roles = "Admin,Staff")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PassengerId,FullName,PhoneNumber,Email,IdentityCard,PassengerType")] Passenger passenger)
    {
        if (await _context.Passengers.AnyAsync(p => p.PassengerId == passenger.PassengerId))
        {
            ModelState.AddModelError("PassengerId", "Mã hành khách đã tồn tại.");
        }

        if (ModelState.IsValid)
        {
            _context.Passengers.Add(passenger);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(passenger);
    }

    // Edit
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var passenger = await _context.Passengers.FindAsync(id);

        if (passenger == null)
        {
            return NotFound();
        }

        return View(passenger);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("PassengerId,FullName,PhoneNumber,Email,IdentityCard,PassengerType")] Passenger passenger)
    {
        if (id != passenger.PassengerId)
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

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(passenger);
    }

    // Delete
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.PassengerId == id);

        if (passenger == null)
        {
            return NotFound();
        }

        return View(passenger);
    }

    [HttpPost]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var passenger = await _context.Passengers.FindAsync(id);

        if (passenger == null)
        {
            return NotFound();
        }

        _context.Passengers.Remove(passenger);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool PassengerExists(string id)
    {
        return _context.Passengers.Any(p => p.PassengerId == id);
    }
}