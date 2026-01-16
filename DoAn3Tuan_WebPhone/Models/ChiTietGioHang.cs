using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class ChiTietGioHang
{
    public int MaCtgh { get; set; }

    public string? MaGioHang { get; set; }

    public string? MaDienThoai { get; set; }

    public int? SoLuong { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual DienThoai? MaDienThoaiNavigation { get; set; }

    public virtual GioHang? MaGioHangNavigation { get; set; }
}
