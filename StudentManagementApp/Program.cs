using StudentManagementApp.BLL;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.Hubs;

namespace StudentManagementApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Index");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapHub<NotificationHub>("/hubs/notifications");

            // Seed admin account from appsettings.json
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
                        Username = config["Username"]!,
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
