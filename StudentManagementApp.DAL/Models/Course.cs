using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApp.DAL.Models;

public class Course
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    public string Level { get; set; } = null!; // A1, A2, B1, B2, C1, C2

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TuitionFee { get; set; }

    [MaxLength(255)]
    public string? ThumbnailUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Class> Classes { get; set; } = new List<Class>();
}
