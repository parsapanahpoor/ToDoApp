using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ToDoApp.Application.Interfaces;
using ToDoApp.Application.Middleware;
using ToDoApp.Application.Services.Account;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infra;
using ToDoApp.Infra.Repositories;
using TaskServiceNew = ToDoApp.Application.Services.Task.TaskServiceNew;
using CategoryServiceNew = ToDoApp.Application.Services.Task.CategoryServiceNew;

namespace ToDoApp;

public class Program
{
    public static void Main(string[] args)
    {
        #region Serilog Configuration

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        #endregion

        try
        {
            Log.Information("Starting web application");

            #region Service Configuration

            var builder = WebApplication.CreateBuilder(args);

            // Add Serilog
            builder.Host.UseSerilog();

            builder.Services.AddControllersWithViews();

            #region AutoMapper

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            #endregion

            #region Add DBContext

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection"));
            });

            #endregion

            #region Repository Registrations

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion

            #region Service Registrations (New Architecture)

            builder.Services.AddScoped<IUserService, UserServiceNew>();
            builder.Services.AddScoped<IRoleService, RoleServiceNew>();
            builder.Services.AddScoped<IAccountService, AccountServiceNew>();
            builder.Services.AddScoped<ITaskService, TaskServiceNew>();
            builder.Services.AddScoped<ICategoryService, CategoryServiceNew>();

            // Old services (kept for backward compatibility if needed)
            builder.Services.AddScoped<RoleService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ToDoApp.Application.Services.Task.CategoryService>();
            builder.Services.AddScoped<ToDoApp.Application.Services.Task.TaskService>();

            #endregion

            #region Authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                // Add Cookie settings
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                });

            #endregion

            #endregion

            #region Pipeline Registration

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Serilog Request Logging
            app.UseSerilogRequestLogging();

            // Language Middleware
            app.UseLanguageMiddleware();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            #endregion
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
