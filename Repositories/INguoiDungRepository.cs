using HealthHub_API.Models;

namespace HealthHub_API.Repositories
{
    public interface INguoiDungRepository
    {
        Task<NguoiDung?> GetByTaiKhoanAsync(string taiKhoan);
        Task<NguoiDung?> GetByEmailAsync(string email);
        Task<bool> TaiKhoanExistsAsync(string taiKhoan);
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(NguoiDung nguoiDung);
        Task UpdateAsync(NguoiDung nguoiDung);
        Task AddAsync(QuanTri doctor);
    }
}
