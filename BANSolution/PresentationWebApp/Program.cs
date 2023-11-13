using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace PresentationWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ShoppingCartDbContext>();
            builder.Services.AddControllersWithViews();

            //absolute path where to store products: C:\Users\attar\source\repos\EP2023_BAN2A\BANSolution\PresentationWebApp\Data\

            var absolutePath = builder.Environment.ContentRootPath + "Data\\products.json";

            builder.Services.AddScoped<IProduct, ProductsJsonRepository>(x=> new ProductsJsonRepository(absolutePath));   //instructing the runtime to inject the ProductsRepository, meaning that
            
            builder.Services.AddScoped(typeof(CategoriesRepository)); //whenever a instance of ProductsRepository is requested, it will be given
                                                                      //the same instance

            //AddScoped => will create AN instance (e.g. of ProductsRepository) one per request
            //             (in a shopping cart context - this is the most efficient one)
            //AddTransient => will create a new instance for every call
            //AddSingleton => will create ONE instance for ALL users!!! 


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}