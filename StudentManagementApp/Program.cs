using StudentManagementApp.BLL;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.Hubs;
using Microsoft.EntityFrameworkCore;
using StudentManagementApp.Services;

namespace StudentManagementApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddControllers();
            builder.Services.AddSignalR();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddScoped<IEnrollmentNotificationPublisher, SignalREnrollmentNotificationPublisher>();
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
            app.MapControllers();
            app.MapHub<NotificationHub>("/hubs/notifications");

            // Seed admin account from appsettings.json
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var config = app.Configuration.GetSection("AdminAccount");

                // Ensure schema is up to date before querying Users table
                await dbContext.Database.MigrateAsync();

                var email = config["Email"]!;
                var username = config["Username"]!;
                var password = config["Password"]!;
                var fullName = config["FullName"]!;
                var phone = config["Phone"];

                var adminByUsername = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
                var adminByEmail = await dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

                if (adminByUsername is null && adminByEmail is null)
                {
                    await authService.RegisterAsync(new CreateUserDto
                    {
                        FullName = fullName,
                        Email = email,
                        Username = username,
                        Phone = phone,
                        Password = password,
                        Role = "Admin"
                    });
                }
                else
                {
                    var admin = adminByUsername ?? adminByEmail!;

                    admin.FullName = fullName;
                    admin.Role = "Admin";
                    admin.IsActive = true;
                    admin.Phone = phone;

                    var hasEmailConflict = await dbContext.Users.AnyAsync(u =>
                        u.Id != admin.Id && u.Email.ToLower() == email.ToLower());
                    if (!hasEmailConflict)
                    {
                        admin.Email = email;
                    }

                    var hasUsernameConflict = await dbContext.Users.AnyAsync(u =>
                        u.Id != admin.Id && u.Username.ToLower() == username.ToLower());
                    if (!hasUsernameConflict)
                    {
                        admin.Username = username;
                    }

                    if (!BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash))
                    {
                        admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                    }

                    await dbContext.SaveChangesAsync();
                }
            }

            app.Run();
        }
    }
}
