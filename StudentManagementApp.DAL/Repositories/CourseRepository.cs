using Microsoft.EntityFrameworkCore;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Course>> GetActiveCoursesAsync() =>
        await _dbSet.Where(c => c.IsActive).ToListAsync();
}
