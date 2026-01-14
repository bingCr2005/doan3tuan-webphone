using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("TaiKhoan")]
[Index("TenDangNhap", Name = "UQ__TaiKhoan__55F68FC09EB133A3", IsUnique = true)]
[Index("Email", Name = "UQ__TaiKhoan__A9D10534576156FD", IsUnique = true)]
public partial class TaiKhoan
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaTaiKhoan { get; set; } = null!;

    [StringLength(50)]
    public string TenDangNhap { get; set; } = null!;

    [StringLength(255)]
    public string MatKhau { get; set; } = null!;

    [StringLength(100)]
    public string HoVaTen { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? SoDienThoai { get; set; }

    public string? DiaChi { get; set; }

    [StringLength(20)]
    public string? LoaiTaiKhoan { get; set; }

    [StringLength(20)]
    public string? TrangThai { get; set; }

    public DateOnly? NgayTao { get; set; }

    [InverseProperty("MaAdminNavigation")]
    public virtual TaiKhoanAdmin? TaiKhoanAdmin { get; set; }

    [InverseProperty("MaKhachHangNavigation")]
    public virtual TaiKhoanKhachHang? TaiKhoanKhachHang { get; set; }
}
