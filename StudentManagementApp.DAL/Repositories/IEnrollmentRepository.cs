using StudentManagementApp.DAL.Models;

namespace StudentManagementApp.DAL.Repositories;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetAllWithDetailsAsync();
    Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Enrollment>> GetByClassIdAsync(int classId);
    Task<Enrollment?> GetByStudentAndClassAsync(int studentId, int classId);
}
