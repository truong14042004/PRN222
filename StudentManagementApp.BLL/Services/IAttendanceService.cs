using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IAttendanceService
{
    Task<IEnumerable<AttendanceDto>> GetByClassAndDateAsync(int classId, DateOnly date);
    Task<IEnumerable<AttendanceDto>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<AttendanceDto>> GetByClassAndStudentAsync(int classId, int studentId);
    Task SaveAttendanceAsync(SaveAttendanceDto dto);
}
