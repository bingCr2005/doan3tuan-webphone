using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models
{
    public partial class TinNhanChat
    {
        public int MaTinNhan { get; set; } // IDENTITY(1,1) 
        public string? MaKhachHang { get; set; } // CHAR(10) [cite: 8]
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? NoiDung { get; set; }
        public DateTime? NgayGui { get; set; }
        public int? TrangThai { get; set; }

        // Liên kết ngược về khách hàng (Navigation Property) [cite: 8, 9]
        public virtual TaiKhoanKhachHang? MaKhachHangNavigation { get; set; }
    }
}