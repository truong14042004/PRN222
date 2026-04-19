using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetByClassAndDateAsync(int classId, DateOnly date);
    Task<IEnumerable<Attendance>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Attendance>> GetByClassAndStudentAsync(int classId, int studentId);
}
