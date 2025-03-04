using System;
using System.Collections.Generic;

namespace HealthHub_API.Models
{
    public partial class QuanTri
    {
        public QuanTri()
        {
            DanhGia = new HashSet<DanhGium>();
            HoiDaps = new HashSet<HoiDap>();
            LichKhams = new HashSet<LichKham>();
        }

        public int IdquanTri { get; set; }
        public string? TaiKhoan { get; set; }
        public string? MatKhau { get; set; }
        public string? ThongTinBacSi { get; set; }
        public string? TrinhDo { get; set; }
        public int? Idkhoa { get; set; }
        public string? HoTen { get; set; }
        public string VaiTro { get; set; } = null!;
        public string? AnhBia { get; set; }
        public string? ThongtinZoom { get; set; }
        public bool? TrangThai { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        public virtual Khoa? IdkhoaNavigation { get; set; }
        public virtual ICollection<DanhGium> DanhGia { get; set; }
        public virtual ICollection<HoiDap> HoiDaps { get; set; }
        public virtual ICollection<LichKham> LichKhams { get; set; }
    }
}
