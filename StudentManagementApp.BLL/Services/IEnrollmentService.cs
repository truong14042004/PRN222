using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDto>> GetAllAsync();
    Task<IEnumerable<EnrollmentDto>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<EnrollmentDto>> GetByClassIdAsync(int classId);
    Task EnrollAsync(int studentId, int classId);
    Task ConfirmAsync(int enrollmentId);
    Task DeleteAsync(int id);
}
