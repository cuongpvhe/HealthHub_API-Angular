using HealthHub_API.Dto;
using HealthHub_API.Helpers;
using HealthHub_API.Models;
using HealthHub_API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;



namespace HealthHub_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly WebAppYteContext _context;
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly IConfiguration _configuration;
        public AuthService(INguoiDungRepository nguoiDungRepository, WebAppYteContext context, IConfiguration configuration)
        {
            _context = context;
            _nguoiDungRepository = nguoiDungRepository;
            _configuration = configuration;
        }

       public async Task<TokenApiDto> AuthenticateAsync(LoginDto loginDto)
{
    if (loginDto == null)
        throw new ArgumentException("Dữ liệu đăng nhập không hợp lệ!");

    object user = null;
    string role = string.Empty;

    // Lấy thông tin Admin từ appsettings.json
    var adminConfig = _configuration.GetSection("AdminAccount");
    string adminUsername = adminConfig["TaiKhoan"];
    string adminPassword = adminConfig["MatKhau"];
    string adminRole = adminConfig["VaiTro"];

    // Nếu là tài khoản admin
    if (loginDto.TaiKhoan == adminUsername)
    {
        var admin = await _context.QuanTris.FirstOrDefaultAsync(x => x.TaiKhoan == adminUsername);

        // Nếu admin chưa tồn tại -> tự động thêm
        if (admin == null)
        {
            var hashedPassword = PasswordHasher.HashPassword(adminPassword);
            admin = new QuanTri
            {
                TaiKhoan = adminUsername,
                MatKhau = hashedPassword,
                HoTen = "Administrator",
                VaiTro = adminRole,
                Token = "",
                RefreshToken = "",
                RefreshTokenExpiry = DateTime.UtcNow.AddSeconds(10)
            };

            await _context.QuanTris.AddAsync(admin);
            await _context.SaveChangesAsync();
        }

        role = "Admin";
        user = admin;
    }
    else
    {
        // Kiểm tra trong bảng NguoiDung trước
        var nguoiDung = await _context.NguoiDungs.FirstOrDefaultAsync(x => x.TaiKhoan == loginDto.TaiKhoan);
        if (nguoiDung != null)
        {
            role = "User";
            user = nguoiDung;
        }
        else
        {
            // Nếu không phải User, kiểm tra trong bảng QuanTri (Doctor/Admin)
            var quanTri = await _context.QuanTris.FirstOrDefaultAsync(x => x.TaiKhoan == loginDto.TaiKhoan);
            if (quanTri != null)
            {
                role = quanTri.VaiTro; // Lấy vai trò từ bảng QuanTri (Admin hoặc Doctor)
                user = quanTri;
            }
        }
    }

    // Nếu không tìm thấy user nào
    if (user == null)
        throw new UnauthorizedAccessException("Tài khoản không tồn tại!");

    // Lấy mật khẩu đã lưu trong DB
    string storedPassword = user switch
    {
        NguoiDung nguoiDungEntity => nguoiDungEntity.MatKhau,
        QuanTri quanTriEntity => quanTriEntity.MatKhau,
        _ => null
    };

    if (string.IsNullOrEmpty(storedPassword) || !PasswordHasher.VerifyPassword(loginDto.MatKhau, storedPassword))
        throw new UnauthorizedAccessException("Mật khẩu không chính xác!");

    // Tạo JWT token và refresh token
    string token = CreateJwtToken(user, role);
    string refreshToken = CreateRefreshToken();

    // Cập nhật token vào database
    switch (user)
    {
        case NguoiDung nguoiDungEntity:
            nguoiDungEntity.Token = token;
            nguoiDungEntity.RefreshToken = refreshToken;
            nguoiDungEntity.RefreshTokenExpiry = DateTime.UtcNow.AddSeconds(10);
            break;

        case QuanTri quanTriEntity:
            quanTriEntity.Token = token;
            quanTriEntity.RefreshToken = refreshToken;
            quanTriEntity.RefreshTokenExpiry = DateTime.UtcNow.AddSeconds(10);
            break;
    }

    await _context.SaveChangesAsync();

    return new TokenApiDto
    {
        Token = token,
        RefreshToken = refreshToken,
        VaiTro = role // Trả về VaiTro để giao diện hiển thị đúng
    };
}


        public async Task<string> RegisterNguoiDungAsync(RegisterNguoiDungDto registerDto)
        {
            if (await _nguoiDungRepository.TaiKhoanExistsAsync(registerDto.TaiKhoan))
                return "Tài khoản đã tồn tại!";

            if (await _nguoiDungRepository.EmailExistsAsync(registerDto.Email))
                return "Email đã tồn tại!";

            var hashedPassword = PasswordHasher.HashPassword(registerDto.MatKhau);

            var nguoiDung = new NguoiDung
            {
                TaiKhoan = registerDto.TaiKhoan,
                MatKhau = hashedPassword,
                HoTen = registerDto.HoTen,
                Email = registerDto.Email,
                RefreshToken = "",
                RefreshTokenExpiry = DateTime.UtcNow.AddSeconds(10)
            };

            await _nguoiDungRepository.AddAsync(nguoiDung);

            return "Đăng ký thành công!";
        }
        public async Task<string> RegisterDoctorAsync(RegisterDoctorDto registerDto)
        {
            // Kiểm tra xem tài khoản đã tồn tại chưa
            if (await _nguoiDungRepository.TaiKhoanExistsAsync(registerDto.TaiKhoan))
                return "Tài khoản đã tồn tại!";

            

            // Hash mật khẩu
            var hashedPassword = PasswordHasher.HashPassword(registerDto.MatKhau);

            // Tạo đối tượng QuanTri cho bác sĩ
            var doctor = new QuanTri
            {
                TaiKhoan = registerDto.TaiKhoan,
                MatKhau = hashedPassword,
                HoTen = registerDto.HoTen,
                
                VaiTro = "Doctor", // Đặt vai trò cho bác sĩ là "Doctor"
                RefreshToken = "",
                RefreshTokenExpiry = DateTime.UtcNow.AddSeconds(10)
            };

            // Lưu bác sĩ vào cơ sở dữ liệu
            await _nguoiDungRepository.AddAsync(doctor);
            return "Đăng ký bác sĩ thành công!";
        }


        private string CreateJwtToken(object user, string role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Đảm bảo khóa bí mật có ít nhất 32 ký tự
            var key = Encoding.UTF8.GetBytes("veryverysceret...............................................");

            var identity = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user is NguoiDung ? ((NguoiDung)user).IdnguoiDung.ToString()
                                                               : ((QuanTri)user).IdquanTri.ToString()),
        new Claim(ClaimTypes.Name, user is NguoiDung ? ((NguoiDung)user).HoTen
                                                     : ((QuanTri)user).HoTen),
        new Claim(ClaimTypes.Role, role)
    });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
        private string CreateRefreshToken()
        {
            string refreshToken;
            do
            {
                var tokenBytes = RandomNumberGenerator.GetBytes(32); // Giảm từ 64 -> 32 bytes
                refreshToken = Convert.ToBase64String(tokenBytes);
            }
            while (_context.NguoiDungs.Any(u => u.RefreshToken == refreshToken) ||
                   _context.QuanTris.Any(u => u.RefreshToken == refreshToken)); // Kiểm tra cả 2 bảng

            return refreshToken;
        }




        //private ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        //{
        //    var key = Encoding.UTF8.GetBytes("veryverysceret...............................................");
        //    var tokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateAudience = false,
        //        ValidateIssuer = false,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ValidateLifetime = false // Cho phép kiểm tra token đã hết hạn
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    SecurityToken securityToken;
        //    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        //    var jwtSecurityToken = securityToken as JwtSecurityToken;

        //    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //        throw new SecurityTokenException("Token không hợp lệ!");

        //    return principal;
        //}

        //private string CreateRefreshToken()
        //{
        //    return Guid.NewGuid().ToString();
        //}

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var nguoiDung = await _nguoiDungRepository.GetByEmailAsync(forgotPasswordDto.Email);
            if (nguoiDung == null)
                return "Email không tồn tại!";

            // Mã hóa mật khẩu mới
            nguoiDung.MatKhau = PasswordHasher.HashPassword(forgotPasswordDto.NewPassword);
            await _nguoiDungRepository.UpdateAsync(nguoiDung);

            return "Mật khẩu đã được đặt lại thành công!";
        }

    }
}
