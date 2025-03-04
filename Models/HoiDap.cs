using System;
using System.Collections.Generic;

namespace HealthHub_API.Models
{
    public partial class HoiDap
    {
        public int Idhoidap { get; set; }
        public string? CauHoi { get; set; }
        public string? TraLoi { get; set; }
        public int? IdnguoiDung { get; set; }
        public int? IdquanTri { get; set; }
        public DateTime? NgayGui { get; set; }
        public string? GhiChu { get; set; }
        public int? TrangThai { get; set; }

        public virtual NguoiDung? IdnguoiDungNavigation { get; set; }
        public virtual QuanTri? IdquanTriNavigation { get; set; }
    }
}
