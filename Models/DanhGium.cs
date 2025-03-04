using System;
using System.Collections.Generic;

namespace HealthHub_API.Models
{
    public partial class DanhGium
    {
        public int IddanhGia { get; set; }
        public string? NoiDung { get; set; }
        public int? IddanhGiaChatLuong { get; set; }
        public int? IdnguoiDung { get; set; }
        public int? IdquanTri { get; set; }
        public int? IdlichKham { get; set; }

        public virtual DanhGiaChatLuong? IddanhGiaChatLuongNavigation { get; set; }
        public virtual LichKham? IdlichKhamNavigation { get; set; }
        public virtual NguoiDung? IdnguoiDungNavigation { get; set; }
        public virtual QuanTri? IdquanTriNavigation { get; set; }
    }
}
