using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public class ClassSchedule
{
    public int Id { get; set; }

    [ForeignKey(nameof(Class))]
    public int ClassId { get; set; }

    [Required]
    [Range(2, 8)]
    public int DayOfWeek { get; set; } // 2=Thứ 2 ... 8=CN

    [Required]
    public TimeOnly StartTime { get; set; }

    [Required]
    public TimeOnly EndTime { get; set; }

    public Class Class { get; set; } = null!;
}
