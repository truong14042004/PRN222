using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpClient();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IOtpRepository, OtpRepository>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPurchasableItemService, PurchasableItemService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IOtpService, OtpService>();

        // Quiz Services
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IQuizQuestionService, QuizQuestionService>();
        services.AddScoped<IQuizResultService, QuizResultService>();
        services.AddScoped<ICourseProgressService, CourseProgressService>();
        services.AddScoped<IQuizHintService, QuizHintService>();

        // Email Service
        services.AddScoped<IEmailService, EmailService>();

        // Payment & E-commerce
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IPurchasableItemService, PurchasableItemService>();

        return services;
    }
}
