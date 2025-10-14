using GymSystemDAL.Repositroies.Classes;
using GymSystemDAL.Repositroies.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystemPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            #region Dependency Injection
            //make dbcontext class puplic
            builder.Services.AddDbContext<GymSystemDAL.Data.Context.GymSystemDBContext>(options =>
            {
                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
                //options.UseSqlServer(builder.Configuration["ConnectionStrings : DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            #endregion

            builder.Services.AddScoped(typeof(GymSystemDAL.Repositroies.Interfaces.IGenericRepository<>), typeof(GymSystemDAL.Repositroies.Classes.GenericRepository<>));
            builder.Services.AddScoped
                <GymSystemDAL.Repositroies.Interfaces.IPlanRepository, GymSystemDAL.Repositroies.Classes.PlanRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
