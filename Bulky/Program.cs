using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositorie;
using Bulky.DataAccess.Repositorie.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Bulky
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// option pour la confirmation d'email avant de se logger : options => options.SignIn.RequireConfirmedAccount = true
			builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

			//injection de dependance(repository)
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // service de gestion des page razor
            builder.Services.AddRazorPages();

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

            // ajouter pour authentification. toujour avant l'authorization car authen verifie les
            // infos de login et autho gere les roles et permissions d'acces
            app.UseAuthentication();


            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            // ajouter pour 
			app.MapRazorPages();

			app.Run();
        }
    }
}
