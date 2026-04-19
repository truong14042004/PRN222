using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public interface IClassRepository : IRepository<Class>
{
    Task<IEnumerable<Class>> GetByTeacherIdAsync(int teacherId);
    Task<IEnumerable<Class>> GetByCourseIdAsync(int courseId);
    Task<IEnumerable<Class>> GetWithSchedulesAsync();
    Task<Class?> GetWithDetailsAsync(int id);
    Task<bool> HasScheduleConflictAsync(int teacherId, int dayOfWeek, TimeOnly startTime, TimeOnly endTime, int? excludeClassId = null);
}
