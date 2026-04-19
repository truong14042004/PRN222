using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
}
