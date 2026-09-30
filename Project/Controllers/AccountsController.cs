using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Controllers;

[Authorize(Roles = "Admin")]
public class AccountsController : Controller
{
    private readonly HanoiMetroDbContext _context;
    private readonly PasswordHasher<Account> _passwordHasher = new();

    public AccountsController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // Index
    public async Task<IActionResult> Index()
    {
        return View(await _context.Accounts
            .Include(a => a.Station)
            .ToListAsync());
    }

    // Details
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts
            .Include(a => a.Station)
            .FirstOrDefaultAsync(a => a.AccountId == id);

        if (account == null)
        {
            return NotFound();
        }

        return View(account);
    }

    // Create
    public IActionResult Create()
    {
        ViewBag.Stations = _context.Stations.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Username,PasswordHash,Role,StationId")]
        Account account)
    {
        if (await _context.Accounts.AnyAsync(a => a.Username == account.Username))
        {
            ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
        }

        if (account.Role != "Admin" && account.Role != "Staff")
        {
            ModelState.AddModelError("Role", "Role chỉ được là Admin hoặc Staff.");
        }

        if (string.IsNullOrWhiteSpace(account.PasswordHash))
        {
            ModelState.AddModelError("PasswordHash", "Mật khẩu không được để trống.");
        }

        if (ModelState.IsValid)
        {
            account.AccountId = Guid.NewGuid();
            account.PasswordHash = _passwordHasher.HashPassword(account, account.PasswordHash);

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Stations = _context.Stations.ToList();
        return View(account);
    }

    // Edit
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }

        ViewBag.Stations = _context.Stations.ToList();

        return View(account);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        Guid id,
        [Bind("AccountId,Username,PasswordHash,Role,StationId")]
        Account account)
    {
        if (id != account.AccountId)
        {
            return NotFound();
        }

        var oldAccount = await _context.Accounts.FindAsync(id);

        if (oldAccount == null)
        {
            return NotFound();
        }

        if (await _context.Accounts.AnyAsync(
            a => a.Username == account.Username &&
                 a.AccountId != account.AccountId))
        {
            ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
        }

        if (account.Role != "Admin" && account.Role != "Staff")
        {
            ModelState.AddModelError("Role", "Role chỉ được là Admin hoặc Staff.");
        }

        if (ModelState.IsValid)
        {
            oldAccount.Username = account.Username;
            oldAccount.Role = account.Role;
            oldAccount.StationId = account.StationId;

            if (!string.IsNullOrWhiteSpace(account.PasswordHash))
            {
                oldAccount.PasswordHash =
                    _passwordHasher.HashPassword(oldAccount, account.PasswordHash);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Stations = _context.Stations.ToList();
        return View(account);
    }

    // Delete
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts
            .Include(a => a.Station)
            .FirstOrDefaultAsync(a => a.AccountId == id);

        if (account == null)
        {
            return NotFound();
        }

        return View(account);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool AccountExists(Guid id)
    {
        return _context.Accounts.Any(a => a.AccountId == id);
    }
}