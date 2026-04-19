using StudentManagementApp.BLL;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Seed admin account từ appsettings.json
            using (var scope = app.Services.CreateScope())
            {
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var config = app.Configuration.GetSection("AdminAccount");

                var email = config["Email"]!;
                var existing = await userService.GetAllAsync();
                if (!existing.Any(u => u.Email == email))
                {
                    await authService.RegisterAsync(new CreateUserDto
                    {
                        FullName = config["FullName"]!,
                        Email = email,
                        Phone = config["Phone"],
                        Password = config["Password"]!,
                        Role = "Admin"
                    });
                }
            }

            app.Run();
        }
    }
}
