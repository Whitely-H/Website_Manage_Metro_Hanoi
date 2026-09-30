using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Security.Claims;

namespace Project.Controllers;

public class AccountController : Controller
{
    private readonly HanoiMetroDbContext _context;
    private readonly PasswordHasher<Account> _passwordHasher;

    public AccountController(HanoiMetroDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<Account>();
    }
    [AllowAnonymous]
    // Login
    [HttpGet]
    public IActionResult Login(string? role)
    {
        return View(new LoginViewModel
        {
            Role = role ?? ""
        });
    }
    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Khách
        if (model.Role == "Guest")
        {
            var guestClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Guest"),
                new Claim(ClaimTypes.Role, "Guest")
            };

            var guestIdentity = new ClaimsIdentity(
                guestClaims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(guestIdentity));

            return RedirectToAction("Index", "Home");
        }

        // Chỉ cho phép Staff hoặc Admin
        if (model.Role != "Staff" && model.Role != "Admin")
        {
            ModelState.AddModelError("", "Vai trò không hợp lệ.");
            return View(model);
        }

        // Tìm tài khoản
        var account = await _context.Accounts
            .FirstOrDefaultAsync(a =>
                a.Username == model.Username &&
                a.Role == model.Role);

        if (account == null)
        {
            ModelState.AddModelError(
                "",
                "Tên đăng nhập hoặc vai trò không đúng.");

            return View(model);
        }

        // Kiểm tra mật khẩu
        var result = _passwordHasher.VerifyHashedPassword(
            account,
            account.PasswordHash,
            model.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(
                "",
                "Mật khẩu không đúng.");

            return View(model);
        }

        // Tạo quyền đăng nhập
        var accountClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.Username),
            new Claim(ClaimTypes.Role, account.Role),
            new Claim(
                ClaimTypes.NameIdentifier,
                account.AccountId.ToString())
        };

        if (!string.IsNullOrEmpty(account.StationId))
        {
            accountClaims.Add(
                new Claim("StationId", account.StationId));
        }

        var accountIdentity = new ClaimsIdentity(
            accountClaims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(accountIdentity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        return RedirectToAction("Index", "Home");
    }

    // Logout
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login", "Account");
    }
    // AccessDenied
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}