using BooksApp_Spring2024_sec01.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace BooksApp_Spring2024_sec01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //fetch connection string
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<BooksDbContext>(options => options.UseSqlServer(connString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BooksDbContext>().AddDefaultTokenProviders();

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IEmailSender, EmailSender>();

            //builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<BooksDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{Area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}