using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class HoaDon
{
    public string MaHoaDon { get; set; } = null!;

    public string? MaKhachHang { get; set; }

    public string? MaAdmin { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    public DateOnly? NgayLap { get; set; }

    public string? MaKhuyenMai { get; set; }

    public decimal? TongTien { get; set; }

    public int? TrangThai { get; set; }

    public string? TenNguoiNhan { get; set; }

    public string? SoDienThoai { get; set; }

    public string? DiaChiGiaoHang { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual TaiKhoanAdmin? MaAdminNavigation { get; set; }

    public virtual TaiKhoanKhachHang? MaKhachHangNavigation { get; set; }

    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
}
