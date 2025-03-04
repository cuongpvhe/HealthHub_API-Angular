using System;
using System.Collections.Generic;

namespace HealthHub_API.Models
{
    public partial class NguoiDung
    {
        public NguoiDung()
        {
            DanhGia = new HashSet<DanhGium>();
            HoiDaps = new HashSet<HoiDap>();
            LichKhams = new HashSet<LichKham>();
        }

        public int IdnguoiDung { get; set; }
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }
        public string? TaiKhoan { get; set; }
        public string? MatKhau { get; set; }
        public int? IdgioiTinh { get; set; }
        public string? DiaChiCuThe { get; set; }
        public int? SoCmnd { get; set; }
        public int? Idtinh { get; set; }
        public string? NhomMau { get; set; }
        public string? ThongTinKhac { get; set; }
        public string? AnhDaiDien { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public bool? TrangThai { get; set; }

        public virtual GioiTinh? IdgioiTinhNavigation { get; set; }
        public virtual TinhThanh? IdtinhNavigation { get; set; }
        public virtual ICollection<DanhGium> DanhGia { get; set; }
        public virtual ICollection<HoiDap> HoiDaps { get; set; }
        public virtual ICollection<LichKham> LichKhams { get; set; }
    }
}
