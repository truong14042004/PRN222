using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public class Attendance
{
    public int Id { get; set; }

    [ForeignKey(nameof(Class))]
    public int ClassId { get; set; }

    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    public bool IsPresent { get; set; } = false;

    [MaxLength(255)]
    public string? Note { get; set; }

    public Class Class { get; set; } = null!;
    public User Student { get; set; } = null!;
}
