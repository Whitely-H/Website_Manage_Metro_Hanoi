using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public OrdersController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // Index
    public async Task<IActionResult> Index()
    {
        return View(await _context.Orders
            .Include(o => o.Passenger)
            .Include(o => o.TicketType)
            .ToListAsync());
    }

    // Details
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(o => o.Passenger)
            .Include(o => o.TicketType)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // Create
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Passengers = await _context.Passengers.ToListAsync();
        ViewBag.TicketTypes = await _context.TicketTypes.ToListAsync();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrderId,PassengerId,TicketTypeId,PurchaseDate,TotalAmount,PaymentMethod")] Order order)
    {
        if (await _context.Orders.AnyAsync(o => o.OrderId == order.OrderId))
        {
            ModelState.AddModelError("OrderId", "Mã đơn hàng đã tồn tại.");
        }

        if (ModelState.IsValid)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Passengers = await _context.Passengers.ToListAsync();
        ViewBag.TicketTypes = await _context.TicketTypes.ToListAsync();
        return View(order);
    }

    // Edit
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        ViewBag.Passengers = await _context.Passengers.ToListAsync();
        ViewBag.TicketTypes = await _context.TicketTypes.ToListAsync();
        return View(order);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("OrderId,PassengerId,TicketTypeId,PurchaseDate,TotalAmount,PaymentMethod")] Order order)
    {
        if (id != order.OrderId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.OrderId))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Passengers = await _context.Passengers.ToListAsync();
        ViewBag.TicketTypes = await _context.TicketTypes.ToListAsync();
        return View(order);
    }

    // Delete
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(o => o.Passenger)
            .Include(o => o.TicketType)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPost]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool OrderExists(string id)
    {
        return _context.Orders.Any(o => o.OrderId == id);
    }
}