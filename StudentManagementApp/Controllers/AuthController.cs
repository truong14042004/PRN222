using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;

namespace StudentManagementApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IOtpService otpService, IUserService userService)
        {
            _authService = authService;
            _otpService = otpService;
            _userService = userService;
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserId") != null)
                return RedirectToAction("Index", "Home");

            ViewBag.LoginError = TempData["LoginError"];
            return View(new LoginDto());
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var user = await _authService.LoginAsync(dto);
            if (user is null)
            {
                ViewBag.LoginError = "Email hoặc mật khẩu không đúng.";
                return View(dto);
            }

            if (!user.IsActive)
            {
                ViewBag.LoginError = "Tài khoản của bạn đã bị vô hiệu hóa. Vui lòng liên hệ trung tâm.";
                return View(dto);
            }

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role);

            return user.Role switch
            {
                "Admin" => RedirectToAction("Index", "Admin"),
                "Teacher" => RedirectToAction("Index", "Teacher"),
                _ => RedirectToAction("Index", "Student")
            };
        }

        // GET: /Auth/Register
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            try
            {
                dto.Role = "Student";
                await _authService.RegisterAsync(dto);
                TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        // GET: /Auth/ForgotPassword
        public IActionResult ForgotPassword() => View();

        // POST: /Auth/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                ViewBag.Error = "Email không hợp lệ.";
                return View();
            }

            var all = await _userService.GetAllAsync();
            var exists = all.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (!exists)
            {
                ViewBag.Error = "Không tìm thấy tài khoản với email này.";
                return View();
            }

            var code = await _otpService.GenerateOtpAsync(email);
            // TODO: gửi email thật — hiện tại hiện OTP ra TempData để demo
            TempData["OtpDemo"] = $"OTP của bạn là: {code}";
            TempData["Email"] = email;
            return RedirectToAction("ResetPassword");
        }

        // GET: /Auth/ResetPassword
        public IActionResult ResetPassword()
        {
            ViewBag.Email = TempData["Email"];
            ViewBag.OtpDemo = TempData["OtpDemo"];
            return View();
        }

        // POST: /Auth/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var valid = await _otpService.ValidateOtpAsync(dto.Email, dto.OtpCode);
            if (!valid)
            {
                ModelState.AddModelError("", "OTP không hợp lệ hoặc đã hết hạn.");
                return View(dto);
            }

            await _authService.ResetPasswordAsync(dto);
            TempData["Success"] = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // POST: /Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
