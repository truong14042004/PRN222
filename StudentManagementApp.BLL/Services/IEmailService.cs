using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailDto email);
    Task<bool> SendOtpAsync(string email, string otpCode, string type);
    Task<bool> SendRegistrationConfirmationAsync(string fullName, string username, string email);
    Task<bool> SendScheduleNotificationAsync(ScheduleEmailDto schedule);
    Task<string> GenerateOtpAsync(string email, string type);
    Task<bool> ValidateOtpAsync(string email, string otpCode, string type);
}
