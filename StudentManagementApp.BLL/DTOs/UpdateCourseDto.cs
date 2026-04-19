using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.BLL.DTOs;

public class UpdateCourseDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên khóa học không được để trống.")]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Cấp độ không được để trống.")]
    public string Level { get; set; } = null!;

    public string? Description { get; set; }

    [Range(0, 100_000_000, ErrorMessage = "Học phí không hợp lệ.")]
    public decimal TuitionFee { get; set; }

    public string? ThumbnailUrl { get; set; }
    public bool IsActive { get; set; }
}
