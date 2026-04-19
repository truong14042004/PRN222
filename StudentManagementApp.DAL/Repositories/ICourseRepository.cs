using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetActiveCoursesAsync();
}
