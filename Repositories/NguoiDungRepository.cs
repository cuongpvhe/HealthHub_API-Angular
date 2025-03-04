using HealthHub_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace HealthHub_API.Repositories
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly WebAppYteContext _context;

        public NguoiDungRepository(WebAppYteContext context)
        {
            _context = context;
        }

        public async Task<NguoiDung?> GetByTaiKhoanAsync(string taiKhoan)
        {
            return await _context.NguoiDungs.FirstOrDefaultAsync(x => x.TaiKhoan == taiKhoan);
        }

        public async Task<NguoiDung?> GetByEmailAsync(string email)
        {
            return await _context.NguoiDungs.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> TaiKhoanExistsAsync(string taiKhoan)
        {
            return await _context.NguoiDungs.AnyAsync(x => x.TaiKhoan == taiKhoan);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.NguoiDungs.AnyAsync(x => x.Email == email);
        }

        public async Task AddAsync(NguoiDung nguoiDung)
        {
            await _context.NguoiDungs.AddAsync(nguoiDung);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NguoiDung nguoiDung)
        {
            _context.NguoiDungs.Update(nguoiDung);
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(QuanTri doctor)
        {
            await _context.QuanTris.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }
    }
}
