using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class TaiKhoanKhachHang
{
    public string MaKhachHang { get; set; } = null!;

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual GioHang? GioHang { get; set; }

    public virtual ICollection<GioHangBackup20260116> GioHangBackup20260116s { get; set; } = new List<GioHangBackup20260116>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual TaiKhoan MaKhachHangNavigation { get; set; } = null!;
}
