using System;
using System.Collections.Generic;

namespace HealthHub_API.Models
{
    public partial class DanhGiaChatLuong
    {
        public DanhGiaChatLuong()
        {
            DanhGia = new HashSet<DanhGium>();
        }

        public int IddanhGiaChatLuong { get; set; }
        public string? DanhGiaChatLuong1 { get; set; }

        public virtual ICollection<DanhGium> DanhGia { get; set; }
    }
}
