using Microsoft.EntityFrameworkCore;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Enrollment>> GetAllWithDetailsAsync() =>
        await _dbSet
            .Include(e => e.Student)
            .Include(e => e.Class)
                .ThenInclude(c => c.Course)
            .ToListAsync();

    public async Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId) =>
        await _dbSet.Where(e => e.StudentId == studentId)
                    .Include(e => e.Class)
                        .ThenInclude(c => c.Course)
                    .Include(e => e.Class)
                        .ThenInclude(c => c.Schedules)
                    .ToListAsync();

    public async Task<IEnumerable<Enrollment>> GetByClassIdAsync(int classId) =>
        await _dbSet.Where(e => e.ClassId == classId)
                    .Include(e => e.Student)
                    .ToListAsync();

    public async Task<Enrollment?> GetByStudentAndClassAsync(int studentId, int classId) =>
        await _dbSet.FirstOrDefaultAsync(e => e.StudentId == studentId && e.ClassId == classId);
}
