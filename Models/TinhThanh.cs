using System;
using System.Collections.Generic;

namespace HealthHub_API.Models
{
    public partial class TinhThanh
    {
        public TinhThanh()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }

        public int Idtinh { get; set; }
        public string? TenTinh { get; set; }

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
