using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Microsoft.AspNetCore.Authorization;

public class TicketTypesController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public TicketTypesController(HanoiMetroDbContext context)
    {
        _context = context;
    }
    // INDEX
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var ticketTypes = await _context.TicketTypes
            .OrderBy(t => t.TicketTypeId)
            .ToListAsync();

        return View(ticketTypes);
    }

    // DETAILS
    [Authorize]
    public async Task<IActionResult> Details(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var ticketType = await _context.TicketTypes
            .FirstOrDefaultAsync(t => t.TicketTypeId == id);

        if (ticketType == null)
        {
            return NotFound();
        }

        return View(ticketType);
    }
    // Buy
    [AllowAnonymous]
    public async Task<IActionResult> Buy(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ticket = await _context.TicketTypes.FirstOrDefaultAsync(t => t.TicketTypeId == id);

        if (ticket == null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Buy(string id, string paymentMethod)
    {
        var ticket = await _context.TicketTypes.FirstOrDefaultAsync(t => t.TicketTypeId == id);
        if (ticket == null) return NotFound();

        if (string.IsNullOrEmpty(paymentMethod))
        {
            ModelState.AddModelError("PaymentMethod", "Vui lòng chọn phương thức thanh toán.");
            return View(ticket);
        }

        // Tạo mã đơn hàng ngẫu nhiên đơn giản
        string newOrderId = "ORD" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);

        var order = new Order
        {
            OrderId = newOrderId,
            PassengerId = null,
            TicketTypeId = ticket.TicketTypeId,
            PurchaseDate = DateTime.Now,
            TotalAmount = ticket.Price,
            PaymentMethod = paymentMethod
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return RedirectToAction("PaymentSuccess", new { orderId = order.OrderId });
    }

    [AllowAnonymous]
    public async Task<IActionResult> PaymentSuccess(string orderId)
    {
        var order = await _context.Orders
            .Include(o => o.TicketType)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null) return NotFound();

        // Nội dung ngẫu nhiên bên trong mã QR
        ViewBag.QrData = "METRO-" + order.OrderId + "-" + Guid.NewGuid().ToString().Substring(0, 8);

        return View(order);
    }
    // CREATE
    [Authorize(Roles = "Staff,Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // CREATE - POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("TicketTypeId,TypeName,Price,DurationHours")]
        TicketType ticketType)
    {
        bool exists = await _context.TicketTypes
            .AnyAsync(t => t.TicketTypeId == ticketType.TicketTypeId);

        if (exists)
        {
            ModelState.AddModelError(
                "TicketTypeId",
                "Mã loại vé đã tồn tại.");
        }

        if (ticketType.Price < 0)
        {
            ModelState.AddModelError(
                "Price",
                "Giá vé không được nhỏ hơn 0.");
        }
        if (ticketType.DurationHours < 0)
        {
            ModelState.AddModelError(
                "DurationHours",
                "Thời hạn không được nhỏ hơn 0.");
        }

        if (ModelState.IsValid)
        {
            _context.TicketTypes.Add(ticketType);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(ticketType);
    }
    // EDIT
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Edit(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var ticketType = await _context.TicketTypes
            .FindAsync(id);

        if (ticketType == null)
        {
            return NotFound();
        }

        return View(ticketType);
    }

    // EDIT 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        string id,
        [Bind("TicketTypeId,TypeName,Price,DurationHours")]
        TicketType ticketType)
    {
        if (id != ticketType.TicketTypeId)
        {
            return NotFound();
        }

        if (ticketType.Price < 0)
        {
            ModelState.AddModelError(
                "Price",
                "Giá vé không được nhỏ hơn 0.");
        }
        if (ticketType.DurationHours < 0)
        {
            ModelState.AddModelError(
                "DurationHours",
                "Thời hạn không được nhỏ hơn 0.");
        }

        if (!ModelState.IsValid)
        {
            return View(ticketType);
        }

        try
        {
            _context.TicketTypes.Update(ticketType);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TicketTypeExists(ticketType.TicketTypeId))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // DELETE - GET
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Delete(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var ticketType = await _context.TicketTypes
            .FirstOrDefaultAsync(t => t.TicketTypeId == id);

        if (ticketType == null)
        {
            return NotFound();
        }

        return View(ticketType);
    }

    // DELETE 
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var ticketType = await _context.TicketTypes
            .FindAsync(id);

        if (ticketType == null)
        {
            return NotFound();
        }

        try
        {
            _context.TicketTypes.Remove(ticketType);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError(
                "",
                "Không thể xóa loại vé vì loại vé này đang được sử dụng trong đơn hàng.");

            return View("Delete", ticketType);
        }

        return RedirectToAction(nameof(Index));
    }

    // KIỂM TRA TỒN TẠI
    private bool TicketTypeExists(string ticketTypeId)
    {
        return _context.TicketTypes
            .Any(t => t.TicketTypeId == ticketTypeId);
    }
}