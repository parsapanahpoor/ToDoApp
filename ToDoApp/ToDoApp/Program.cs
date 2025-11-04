using Microsoft.EntityFrameworkCore;
using ToDoApp.Infra;

namespace ToDoApp;

public class Program
{
    public static void Main(string[] args)
    {
        #region Service Configuration

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        #region Add DBContext

        builder.Services.AddDbContext<AppplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection"));
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

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();

        #endregion
    }
}
