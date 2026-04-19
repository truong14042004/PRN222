using Microsoft.EntityFrameworkCore;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public class ClassRepository : Repository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Class>> GetByTeacherIdAsync(int teacherId) =>
        await _dbSet.Where(c => c.TeacherId == teacherId)
                    .Include(c => c.Course)
                    .Include(c => c.Schedules)
                    .ToListAsync();

    public async Task<IEnumerable<Class>> GetByCourseIdAsync(int courseId) =>
        await _dbSet.Where(c => c.CourseId == courseId)
                    .Include(c => c.Teacher)
                    .Include(c => c.Schedules)
                    .ToListAsync();

    public async Task<IEnumerable<Class>> GetWithSchedulesAsync() =>
        await _dbSet.Include(c => c.Course)
                    .Include(c => c.Teacher)
                    .Include(c => c.Schedules)
                    .ToListAsync();

    public async Task<Class?> GetWithDetailsAsync(int id) =>
        await _dbSet.Include(c => c.Course)
                    .Include(c => c.Teacher)
                    .Include(c => c.Schedules)
                    .Include(c => c.Enrollments)
                    .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> HasScheduleConflictAsync(int teacherId, int dayOfWeek, TimeOnly startTime, TimeOnly endTime, int? excludeClassId = null)
    {
        return await _dbSet
            .Where(c => c.TeacherId == teacherId && c.Id != excludeClassId)
            .AnyAsync(c => c.Schedules.Any(s =>
                s.DayOfWeek == dayOfWeek &&
                s.StartTime < endTime &&
                s.EndTime > startTime));
    }
}
