using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentManagementApp.DAL.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=EnglishCenterDB;User ID=sa;Password=1234567890;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True");
        return new AppDbContext(optionsBuilder.Options);
    }
}