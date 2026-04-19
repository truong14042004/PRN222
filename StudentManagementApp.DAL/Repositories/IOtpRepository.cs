using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public interface IOtpRepository : IRepository<OtpCode>
{
    Task<OtpCode?> GetValidOtpAsync(string email, string code);
}
