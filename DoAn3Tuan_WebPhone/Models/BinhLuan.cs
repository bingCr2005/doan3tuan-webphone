using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class BinhLuan
{
    public string MaBinhLuan { get; set; } = null!;

    public string? MaDienThoai { get; set; }

    public string? MaKhachHang { get; set; }

    public int? SoSao { get; set; }

    public string? NoiDung { get; set; }

    public DateOnly? NgayBinhLuan { get; set; }

    public int? TrangThai { get; set; }

    public virtual DienThoai? MaDienThoaiNavigation { get; set; }

    public virtual TaiKhoanKhachHang? MaKhachHangNavigation { get; set; }
}
