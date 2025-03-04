using System;
using System.Collections.Generic;

namespace HealthHub_API.Models
{
    public partial class Tintuc
    {
        public int Idtintuc { get; set; }
        public string? Tieude { get; set; }
        public string? Noidung { get; set; }
        public string? Hinhanh { get; set; }
        public string? Mota { get; set; }
        public DateTime? Ngaydang { get; set; }
        public string? TheLoai { get; set; }
    }
}
