using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class GioHangBackup20260116
{
    public string MaGioHang { get; set; } = null!;

    public string? MaKhachHang { get; set; }

    public string? MaDienThoai { get; set; }

    public int? SoLuongHang { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual DienThoai? MaDienThoaiNavigation { get; set; }

    public virtual TaiKhoanKhachHang? MaKhachHangNavigation { get; set; }
}
