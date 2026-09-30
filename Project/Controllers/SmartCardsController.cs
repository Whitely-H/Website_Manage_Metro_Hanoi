using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers;

[Authorize]
public class SmartCardsController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public SmartCardsController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // Index
    public async Task<IActionResult> Index()
    {
        return View(await _context.SmartCards
            .Include(s => s.Passenger)
            .ToListAsync());
    }

    // Create
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Passengers = await _context.Passengers.ToListAsync();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CardId,PassengerId,NfcCode,Balance,IssueDate")] SmartCard smartCard)
    {
        if (await _context.SmartCards.AnyAsync(s => s.CardId == smartCard.CardId))
        {
            ModelState.AddModelError("CardId", "Mã thẻ đã tồn tại.");
        }

        if (await _context.SmartCards.AnyAsync(s => s.NfcCode == smartCard.NfcCode))
        {
            ModelState.AddModelError("NfcCode", "Mã NFC đã tồn tại.");
        }

        if (ModelState.IsValid)
        {
            _context.SmartCards.Add(smartCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Passengers = await _context.Passengers.ToListAsync();
        return View(smartCard);
    }

    // Edit
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var smartCard = await _context.SmartCards.FindAsync(id);

        if (smartCard == null)
        {
            return NotFound();
        }

        ViewBag.Passengers = await _context.Passengers.ToListAsync();
        return View(smartCard);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("CardId,PassengerId,NfcCode,Balance,IssueDate")] SmartCard smartCard)
    {
        if (id != smartCard.CardId)
        {
            return NotFound();
        }

        if (await _context.SmartCards.AnyAsync(s => s.NfcCode == smartCard.NfcCode && s.CardId != smartCard.CardId))
        {
            ModelState.AddModelError("NfcCode", "Mã NFC đã tồn tại.");
        }

        if (ModelState.IsValid)
        {
            var oldCard = await _context.SmartCards.FindAsync(id);

            if (oldCard == null)
            {
                return NotFound();
            }

            oldCard.PassengerId = smartCard.PassengerId;
            oldCard.NfcCode = smartCard.NfcCode;
            oldCard.Balance = smartCard.Balance;
            oldCard.IssueDate = smartCard.IssueDate;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Passengers = await _context.Passengers.ToListAsync();
        return View(smartCard);
    }

    // Delete
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var smartCard = await _context.SmartCards
            .Include(s => s.Passenger)
            .FirstOrDefaultAsync(s => s.CardId == id);

        if (smartCard == null)
        {
            return NotFound();
        }

        return View(smartCard);
    }

    [HttpPost]
    [ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var smartCard = await _context.SmartCards.FindAsync(id);

        if (smartCard == null)
        {
            return NotFound();
        }

        _context.SmartCards.Remove(smartCard);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}