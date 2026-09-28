
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;

public class AccountsController : Controller
{
    private readonly HanoiMetroDbContext _context;

    public AccountsController(HanoiMetroDbContext context)
    {
        _context = context;
    }

    // GET: ACCOUNTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Accounts.ToListAsync());
    }

    // GET: ACCOUNTS/Details/5
    public async Task<IActionResult> Details(System.Guid? accountid)
    {
        if (accountid == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts
            .FirstOrDefaultAsync(m => m.AccountId == accountid);
        if (account == null)
        {
            return NotFound();
        }

        return View(account);
    }

    // GET: ACCOUNTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ACCOUNTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AccountId,Username,PasswordHash,Role,StationId,Station")] Account account)
    {
        if (ModelState.IsValid)
        {
            _context.Add(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(account);
    }

    // GET: ACCOUNTS/Edit/5
    public async Task<IActionResult> Edit(System.Guid? accountid)
    {
        if (accountid == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts.FindAsync(accountid);
        if (account == null)
        {
            return NotFound();
        }
        return View(account);
    }

    // POST: ACCOUNTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(System.Guid? accountid, [Bind("AccountId,Username,PasswordHash,Role,StationId,Station")] Account account)
    {
        if (accountid != account.AccountId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(account);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(account.AccountId))
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
        return View(account);
    }

    // GET: ACCOUNTS/Delete/5
    public async Task<IActionResult> Delete(System.Guid? accountid)
    {
        if (accountid == null)
        {
            return NotFound();
        }

        var account = await _context.Accounts
            .FirstOrDefaultAsync(m => m.AccountId == accountid);
        if (account == null)
        {
            return NotFound();
        }

        return View(account);
    }

    // POST: ACCOUNTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(System.Guid? accountid)
    {
        var account = await _context.Accounts.FindAsync(accountid);
        if (account != null)
        {
            _context.Accounts.Remove(account);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AccountExists(System.Guid? accountid)
    {
        return _context.Accounts.Any(e => e.AccountId == accountid);
    }
}
