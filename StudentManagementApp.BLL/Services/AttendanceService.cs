using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;

namespace StudentManagementApp.BLL.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceService(IAttendanceRepository attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }

    public async Task<IEnumerable<AttendanceDto>> GetByClassAndDateAsync(int classId, DateOnly date)
    {
        var attendances = await _attendanceRepository.GetByClassAndDateAsync(classId, date);
        return attendances.Select(MapToDto);
    }

    public async Task<IEnumerable<AttendanceDto>> GetByStudentIdAsync(int studentId)
    {
        var attendances = await _attendanceRepository.GetByStudentIdAsync(studentId);
        return attendances.Select(MapToDto);
    }

    public async Task<IEnumerable<AttendanceDto>> GetByClassAndStudentAsync(int classId, int studentId)
    {
        var attendances = await _attendanceRepository.GetByClassAndStudentAsync(classId, studentId);
        return attendances.Select(MapToDto);
    }

    public async Task SaveAttendanceAsync(SaveAttendanceDto dto)
    {
        var existing = await _attendanceRepository.GetByClassAndDateAsync(dto.ClassId, dto.Date);

        foreach (var item in dto.Items)
        {
            var record = existing.FirstOrDefault(a => a.StudentId == item.StudentId);
            if (record is null)
            {
                await _attendanceRepository.AddAsync(new Attendance
                {
                    ClassId = dto.ClassId,
                    StudentId = item.StudentId,
                    Date = dto.Date,
                    IsPresent = item.IsPresent,
                    Note = item.Note
                });
            }
            else
            {
                record.IsPresent = item.IsPresent;
                record.Note = item.Note;
                await _attendanceRepository.UpdateAsync(record);
            }
        }
    }

    private static AttendanceDto MapToDto(Attendance a) => new()
    {
        Id = a.Id,
        ClassId = a.ClassId,
        StudentId = a.StudentId,
        StudentName = a.Student?.FullName ?? string.Empty,
        Date = a.Date,
        IsPresent = a.IsPresent,
        Note = a.Note
    };
}
