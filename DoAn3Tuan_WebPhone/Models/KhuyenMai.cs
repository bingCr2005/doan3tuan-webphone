using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class KhuyenMai
{
    public string MaKhuyenMai { get; set; } = null!;

    public string? TenChuongTrinhKhuyenMai { get; set; }

    public decimal? MucGiamGia { get; set; }

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayHetHan { get; set; }

    public string? DieuKienApDung { get; set; }

    public int? SoLuongMaKhuyenMaiDaDung { get; set; }

    public int? TrangThai { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
