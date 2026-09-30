using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // Kết nối database
            var prjConnection = builder.Configuration
                .GetConnectionString("HanoiMetroDBConnection");

            builder.Services.AddDbContext<HanoiMetroDbContext>(x =>
                x.UseSqlServer(prjConnection));

            // Authentication bằng Cookie
            builder.Services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Phải đặt Authentication trước Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}