using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[PrimaryKey("MaHoaDon", "MaDienThoai")]
[Table("ChiTietHoaDon")]
public partial class ChiTietHoaDon
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaHoaDon { get; set; } = null!;

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDienThoai { get; set; } = null!;

    public string? DiaChi { get; set; }

    public int? SoLuong { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? DonGia { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhuyenMai { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ThanhTien { get; set; }

    [StringLength(50)]
    public string? PhuongThucThanhToan { get; set; }

    public int? TrangThai { get; set; }

    [ForeignKey("MaDienThoai")]
    [InverseProperty("ChiTietHoaDons")]
    public virtual DienThoai MaDienThoaiNavigation { get; set; } = null!;

    [ForeignKey("MaHoaDon")]
    [InverseProperty("ChiTietHoaDons")]
    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;

    [ForeignKey("MaKhuyenMai")]
    [InverseProperty("ChiTietHoaDons")]
    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
}
