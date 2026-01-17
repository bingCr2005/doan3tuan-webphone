using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class ChiTietHoaDon
{
    public string MaHoaDon { get; set; } = null!;

    public string MaDienThoai { get; set; } = null!;

    public string? DiaChi { get; set; }

    public int? SoLuong { get; set; }

    public decimal? DonGia { get; set; }

    public string? MaKhuyenMai { get; set; }

    public decimal? ThanhTien { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    public int? TrangThai { get; set; }

    public virtual DienThoai MaDienThoaiNavigation { get; set; } = null!;

    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
}
