using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class TaiKhoanKhachHang
{
    public string MaKhachHang { get; set; } = null!;

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual GioHang? GioHang { get; set; }

    public virtual ICollection<GioHangOld> GioHangOlds { get; set; } = new List<GioHangOld>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual TaiKhoan MaKhachHangNavigation { get; set; } = null!;
}
