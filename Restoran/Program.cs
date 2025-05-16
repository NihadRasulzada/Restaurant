using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restoran.DAL.Contexts;
using Restoran.DAL.Validatos.ChefDesignationDtoValidator;
using Restoran.DAL.Validatos.MemberDesignationDtoValidator;
using Restoran.DAL.Validatos.MemberDtoValidator;
using Restoran.Models.Account;
using SafeCam.MVC.DAL.Validatos.MemberDtoValidator;
using Softy_Pinko.Services;

namespace Restoran
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddServices();

            builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            builder.Services.AddValidatorsFromAssemblyContaining<CreateChefDesignationDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateChefDesignationDtoValidator>();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateChefDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateChefDtoValidator>();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
