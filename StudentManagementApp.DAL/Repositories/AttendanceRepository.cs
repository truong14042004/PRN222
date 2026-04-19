using Microsoft.EntityFrameworkCore;
using StudentManagementApp.DAL.Data;
using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Attendance>> GetByClassAndDateAsync(int classId, DateOnly date) =>
        await _dbSet.Where(a => a.ClassId == classId && a.Date == date)
                    .Include(a => a.Student)
                    .ToListAsync();

    public async Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId) =>
        await _dbSet.Where(a => a.StudentId == studentId)
                    .Include(a => a.Class)
                    .ToListAsync();

    public async Task<IEnumerable<Attendance>> GetByClassAndStudentAsync(int classId, int studentId) =>
        await _dbSet.Where(a => a.ClassId == classId && a.StudentId == studentId)
                    .ToListAsync();
}
