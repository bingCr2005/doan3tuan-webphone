using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class TaiKhoan
{
    public string MaTaiKhoan { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string HoVaTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? LoaiTaiKhoan { get; set; }

    public string? TrangThai { get; set; }

    public DateOnly? NgayTao { get; set; }

    public virtual TaiKhoanAdmin? TaiKhoanAdmin { get; set; }

    public virtual TaiKhoanKhachHang? TaiKhoanKhachHang { get; set; }
}
