using Microsoft.EntityFrameworkCore;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public class OtpRepository : Repository<OtpCode>, IOtpRepository
{
    public OtpRepository(AppDbContext context) : base(context) { }

    public async Task<OtpCode?> GetValidOtpAsync(string email, string code) =>
        await _dbSet.FirstOrDefaultAsync(o =>
            o.Email == email &&
            o.Code == code &&
            !o.IsUsed &&
            o.ExpiredAt > DateTime.Now);
}
