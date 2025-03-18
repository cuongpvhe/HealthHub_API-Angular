using HealthHub_API.Dto;
using HealthHub_API.Models;
using HealthHub_API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HealthHub_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.AuthenticateAsync(loginDto);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("registerNguoiDung")]
        public async Task<IActionResult> RegisterNguoiDung([FromBody] RegisterNguoiDungDto registerDto)
        {
            var message = await _authService.RegisterNguoiDungAsync(registerDto);
            return Ok(new { Message = message });
        }
        [HttpPost("registerDoctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorDto registerDto)
        {
            // Kiểm tra xem header có chứa Authorization không
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Unauthorized(new { Message = "Không tìm thấy token!" });
            }

            // Lấy token từ header
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { Message = "Token không hợp lệ!" });
            }

            var token = authHeader.Replace("Bearer ", "").Trim();

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "Token không hợp lệ hoặc rỗng!" });
            }

            // Kiểm tra và giải mã token
            var claims = ValidateJwtToken(token);
            if (claims == null)
            {
                return Unauthorized(new { Message = "Token không hợp lệ hoặc đã hết hạn!" });
            }

            // Lấy quyền (role) từ token
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleClaim) || roleClaim != "Admin")
            {
                return Unauthorized(new { Message = "Chỉ có Admin mới được phép đăng ký bác sĩ!" });
            }

            //Tiến hành đăng ký bác sĩ
            var message = await _authService.RegisterDoctorAsync(registerDto);
            return Ok(new { Message = message });
        }
       



        private IEnumerable<Claim> ValidateJwtToken(string token)
        {
            // Logic giải mã token JWT và lấy claims
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            return jsonToken?.Claims;
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPasswordDto);
            return Ok(new { message = result });
        }
    }
}
