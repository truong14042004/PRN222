using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;
using Xunit;

namespace StudentManagementApp.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_returns_null_when_password_is_incorrect()
    {
        var user = CreateUser(password: "CorrectPass123");
        var repository = new FakeUserRepository(user);
        var service = new AuthService(repository);

        var result = await service.LoginAsync(new LoginDto
        {
            Username = user.Username,
            Password = "WrongPass123"
        });

        Assert.Null(result);
    }

    [Fact]
    public async Task ChangePasswordAsync_updates_password_hash_when_current_password_matches()
    {
        var user = CreateUser(password: "OldPass123");
        var repository = new FakeUserRepository(user);
        var service = new AuthService(repository);

        var changed = await service.ChangePasswordAsync(user.Id, "OldPass123", "NewPass456");

        Assert.True(changed);
        Assert.True(BCrypt.Net.BCrypt.Verify("NewPass456", user.PasswordHash));
    }

    [Fact]
    public async Task ChangePasswordAsync_returns_false_when_current_password_is_invalid()
    {
        var user = CreateUser(password: "OldPass123");
        var repository = new FakeUserRepository(user);
        var service = new AuthService(repository);

        var changed = await service.ChangePasswordAsync(user.Id, "WrongPass123", "NewPass456");

        Assert.False(changed);
        Assert.True(BCrypt.Net.BCrypt.Verify("OldPass123", user.PasswordHash));
    }

    private static User CreateUser(string password) => new()
    {
        Id = 42,
        FullName = "Student User",
        Email = "student@example.com",
        Username = "student01",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        Role = "Student",
        IsActive = true
    };

    private sealed class FakeUserRepository : IUserRepository
    {
        private readonly User _user;

        public FakeUserRepository(User user)
        {
            _user = user;
        }

        public Task<IEnumerable<User>> GetAllAsync() => Task.FromResult(Enumerable.Repeat(_user, 1).AsEnumerable());

        public Task<User?> GetByIdAsync(int id) => Task.FromResult(id == _user.Id ? _user : null);

        public Task AddAsync(User entity) => Task.CompletedTask;

        public Task UpdateAsync(User entity) => Task.CompletedTask;

        public Task DeleteAsync(int id) => Task.CompletedTask;

        public Task<User?> GetByEmailAsync(string email) =>
            Task.FromResult(string.Equals(email, _user.Email, StringComparison.OrdinalIgnoreCase) ? _user : null);

        public Task<User?> GetByUsernameAsync(string username) =>
            Task.FromResult(string.Equals(username, _user.Username, StringComparison.OrdinalIgnoreCase) ? _user : null);

        public Task<bool> ExistsByEmailAsync(string email) =>
            Task.FromResult(string.Equals(email, _user.Email, StringComparison.OrdinalIgnoreCase));

        public Task<bool> ExistsByUsernameAsync(string username) =>
            Task.FromResult(string.Equals(username, _user.Username, StringComparison.OrdinalIgnoreCase));
    }
}
