using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.BLL.Services;

public class EmailService : IEmailService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public EmailService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<bool> SendEmailAsync(EmailDto email)
    {
        try
        {
            var smtpHost = _config["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"] ?? "587");
            var smtpUser = _config["EmailSettings:Username"] ?? "";
            var smtpPass = _config["EmailSettings:Password"] ?? "";
            var fromEmail = _config["EmailSettings:FromEmail"] ?? smtpUser;

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail, "English Center"),
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = email.IsHtml
            };
            mail.To.Add(email.To);

            await client.SendMailAsync(mail);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email.To);
            if (user != null)
            {
                _context.EmailLogs.Add(new EmailLog
                {
                    UserId = user.Id,
                    Recipient = email.To,
                    Subject = email.Subject,
                    Body = email.Body,
                    IsSent = true
                });
                await _context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email.To);
            if (user != null)
            {
                _context.EmailLogs.Add(new EmailLog
                {
                    UserId = user.Id,
                    Recipient = email.To,
                    Subject = email.Subject,
                    IsSent = false,
                    ErrorMessage = ex.Message
                });
                await _context.SaveChangesAsync();
            }

            return false;
        }
    }

    public async Task<bool> SendOtpAsync(string email, string otpCode, string type)
    {
        var emailDto = new EmailDto
        {
            To = email,
            Subject = type == "Registration"
                ? "Mã xác thực đăng ký - English Center"
                : "Mã xác thực đặt lại mật khẩu - English Center",
            Body = $@"
                <h2>English Center</h2>
                <p>Mã xác thực của bạn là: <strong style='font-size: 24px; color: #007bff;'>{otpCode}</strong></p>
                <p>Mã có hiệu lực trong 5 phút. Vui lòng không chia sẻ mã này với bất kỳ ai.</p>
                <p>Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email.</p>
            "
        };

        return await SendEmailAsync(emailDto);
    }

    public async Task<bool> SendRegistrationConfirmationAsync(string fullName, string username, string email)
    {
        var emailDto = new EmailDto
        {
            To = email,
            Subject = "Đăng ký tài khoản thành công - English Center",
            Body = $@"
                <h2>Xin chào {fullName},</h2>
                <p>Tài khoản của bạn đã được xác thực và tạo thành công.</p>
                <p><strong>Tên đăng nhập:</strong> {username}</p>
                <p>Bạn có thể đăng nhập ngay để bắt đầu sử dụng hệ thống.</p>
                <p>Trân trọng,<br />English Center</p>
            "
        };

        return await SendEmailAsync(emailDto);
    }

    public async Task<bool> SendScheduleNotificationAsync(ScheduleEmailDto schedule)
    {
        var sessionsHtml = string.Join("", schedule.Sessions.Select(s => $@"
            <tr>
                <td>{s.DayName}<br>{s.Date}</td>
                <td>{s.StartTime} - {s.EndTime}</td>
                <td>{s.ClassName}</td>
                <td>{s.CourseName}</td>
            </tr>
        "));

        var emailDto = new EmailDto
        {
            To = schedule.Email,
            Subject = $"Lịch học tuần {schedule.WeekStart:dd/MM/yyyy} - {schedule.WeekEnd:dd/MM/yyyy}",
            Body = $@"
                <h2>Xin chào {schedule.StudentName},</h2>
                <p>Dưới đây là lịch học của bạn trong tuần từ <strong>{schedule.WeekStart:dd/MM/yyyy}</strong> đến <strong>{schedule.WeekEnd:dd/MM/yyyy}</strong>:</p>

                <table style='border-collapse: collapse; width: 100%;'>
                    <thead>
                        <tr style='background-color: #007bff; color: white;'>
                            <th style='padding: 10px; border: 1px solid #ddd;'>Ngày</th>
                            <th style='padding: 10px; border: 1px solid #ddd;'>Giờ học</th>
                            <th style='padding: 10px; border: 1px solid #ddd;'>Lớp</th>
                            <th style='padding: 10px; border: 1px solid #ddd;'>Khóa học</th>
                        </tr>
                    </thead>
                    <tbody>
                        {sessionsHtml}
                    </tbody>
                </table>

                <p>Chúc bạn học tốt!</p>
                <p>Trung tâm Anh ngữ English Center</p>
            "
        };

        return await SendEmailAsync(emailDto);
    }

    public async Task<string> GenerateOtpAsync(string email, string type)
    {
        if (!Enum.TryParse<OtpType>(type, true, out var otpType))
            throw new InvalidOperationException($"OTP type '{type}' is not supported.");

        var oldOtps = await _context.EmailOtps
            .Where(e => e.Email == email && e.Type == otpType && !e.IsUsed)
            .ToListAsync();
        _context.EmailOtps.RemoveRange(oldOtps);

        var otpCode = new Random().Next(100000, 999999).ToString();
        var otp = new EmailOtp
        {
            Email = email,
            OtpCode = otpCode,
            Type = otpType,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };

        _context.EmailOtps.Add(otp);
        await _context.SaveChangesAsync();

        var isSent = await SendOtpAsync(email, otpCode, type);
        if (!isSent)
        {
            _context.EmailOtps.Remove(otp);
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("Không thể gửi mã OTP lúc này. Vui lòng thử lại sau.");
        }

        return otpCode;
    }

    public async Task<bool> ValidateOtpAsync(string email, string otpCode, string type)
    {
        if (!Enum.TryParse<OtpType>(type, true, out var otpType))
            return false;

        var otp = await _context.EmailOtps
            .Where(e => e.Email == email && e.OtpCode == otpCode && e.Type == otpType && !e.IsUsed)
            .FirstOrDefaultAsync();

        if (otp == null || !otp.IsValid)
            return false;

        otp.IsUsed = true;
        await _context.SaveChangesAsync();

        return true;
    }
}
