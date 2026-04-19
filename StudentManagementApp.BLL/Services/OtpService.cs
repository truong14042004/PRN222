using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class OtpService : IOtpService
{
    private readonly IOtpRepository _otpRepository;

    public OtpService(IOtpRepository otpRepository)
    {
        _otpRepository = otpRepository;
    }

    public async Task<string> GenerateOtpAsync(string email)
    {
        var code = Random.Shared.Next(100000, 999999).ToString();

        await _otpRepository.AddAsync(new OtpCode
        {
            Email = email,
            Code = code,
            ExpiredAt = DateTime.Now.AddMinutes(5),
            IsUsed = false
        });

        return code;
    }

    public async Task<bool> ValidateOtpAsync(string email, string code)
    {
        var otp = await _otpRepository.GetValidOtpAsync(email, code);
        if (otp is null) return false;

        otp.IsUsed = true;
        await _otpRepository.UpdateAsync(otp);
        return true;
    }
}
