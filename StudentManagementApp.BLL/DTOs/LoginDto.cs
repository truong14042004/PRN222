using System.ComponentModel.DataAnnotations;

namespace StudentManagementApp.BLL.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Tên đăng nhập hoặc email không được để trống.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu không được để trống.")]
    [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.")]
    public string Password { get; set; } = null!;
}
