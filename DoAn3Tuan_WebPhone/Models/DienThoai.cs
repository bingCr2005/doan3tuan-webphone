using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class DienThoai
{
    public string MaDienThoai { get; set; } = null!;

    public string TenDienThoai { get; set; } = null!;

    public string? HangDienThoai { get; set; }

    public string? HeDieuHanh { get; set; }

    public string? MauSac { get; set; }

    public string? Ram { get; set; }

    public string? DungLuong { get; set; }

    public string? KichThuocManHinh { get; set; }

    public string? DoPhanGiaiManHinh { get; set; }

    public string? DungLuongPin { get; set; }

    public string? ChatLieu { get; set; }

    public string? MoTa { get; set; }

    public decimal DonGia { get; set; }

    public int? SoLuongTon { get; set; }

    public string? MaNhaCungCap { get; set; }

    public string? Thumbnail { get; set; }

    public int? TrangThai { get; set; }

    public int SoLuongYeuThich { get; set; }

    public int LuotXem { get; set; }

    public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual ICollection<GioHangOld> GioHangOlds { get; set; } = new List<GioHangOld>();

    public virtual HangDienThoai? HangDienThoaiNavigation { get; set; }

    public virtual ICollection<HinhAnh> HinhAnhs { get; set; } = new List<HinhAnh>();

    public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }
}
