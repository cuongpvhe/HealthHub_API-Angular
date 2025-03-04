using HealthHub_API.Dto;
using HealthHub_API.Models;

namespace HealthHub_API.Services
{
    public interface IAuthService
    {
        Task<TokenApiDto?> AuthenticateAsync(LoginDto loginDto);
        Task<string> RegisterNguoiDungAsync(RegisterNguoiDungDto registerDto);
        Task<string> RegisterDoctorAsync(RegisterDoctorDto registerDto);
    }
}
