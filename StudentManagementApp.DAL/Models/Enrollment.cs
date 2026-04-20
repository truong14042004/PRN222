using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public class Enrollment
{
    public int Id { get; set; }

    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }

    [ForeignKey(nameof(Class))]
    public int ClassId { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.Now;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Registered"; // Registered, Confirmed

    [MaxLength(10)]
    public string? ConfirmationCode { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public User Student { get; set; } = null!;
    public Class Class { get; set; } = null!;
}
