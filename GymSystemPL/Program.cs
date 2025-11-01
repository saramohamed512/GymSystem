using GymSystemBLL;
using GymSystemBLL.Services;
using GymSystemBLL.Services.AttachmentService;
using GymSystemBLL.Services.Classes;
using GymSystemBLL.Services.Interfaces;
using GymSystemDAL;
using GymSystemDAL.Data.Context;
using GymSystemDAL.Data.DataSeed;
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
            builder.Services.AddScoped<GymSystemDAL.Repositroies.Interfaces.ISessionRepoitory, GymSystemDAL.Repositroies.Classes.SessionRepoitory>();
            builder.Services.AddAutoMapper(X=>X.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<IMemberService , MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>(
                Config =>
                {

                    Config.Password.RequireLowercase = true;
                    Config.Password.RequireUppercase = true;
                    Config.Password.RequiredLength = 6;
                    Config.User.RequireUniqueEmail = true;
                }

                )
                .AddEntityFrameworkStores<GymSystemDBContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });


            var app = builder.Build();

            #region Data Seed
            var Scope= app.Services.CreateScope();
            var dbContext = Scope.ServiceProvider.GetRequiredService<GymSystemDBContext>();

            //Check if There Is Migrations Pending or Not
            var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();
            GymDbContextSeeding.SeedData(dbContext);

            var RoleManager = Scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
            var UserManager = Scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
            IdentityDbContextSeeding.SeedData(RoleManager, UserManager);
            #endregion

            //Middilewares
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

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
