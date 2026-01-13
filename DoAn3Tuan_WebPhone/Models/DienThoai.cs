using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("DienThoai")]
public partial class DienThoai
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDienThoai { get; set; } = null!;

    [StringLength(200)]
    public string TenDienThoai { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? HangDienThoai { get; set; }

    [StringLength(50)]
    public string? HeDieuHanh { get; set; }

    [StringLength(50)]
    public string? MauSac { get; set; }

    [StringLength(20)]
    public string? RAM { get; set; }

    [StringLength(20)]
    public string? DungLuong { get; set; }

    [StringLength(20)]
    public string? KichThuocManHinh { get; set; }

    [StringLength(50)]
    public string? DoPhanGiaiManHinh { get; set; }

    [StringLength(20)]
    public string? DungLuongPin { get; set; }

    [StringLength(100)]
    public string? ChatLieu { get; set; }

    public string? MoTa { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DonGia { get; set; }

    public int? SoLuongTon { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaNhaCungCap { get; set; }

    [StringLength(500)]
    public string? Thumbnail { get; set; }

    public int? TrangThai { get; set; }

    [InverseProperty("MaDienThoaiNavigation")]
    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    [InverseProperty("MaDienThoaiNavigation")]
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    [InverseProperty("MaDienThoaiNavigation")]
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    [ForeignKey("HangDienThoai")]
    [InverseProperty("DienThoais")]
    public virtual HangDienThoai? HangDienThoaiNavigation { get; set; }

    [InverseProperty("MaDienThoaiNavigation")]
    public virtual ICollection<HinhAnh> HinhAnhs { get; set; } = new List<HinhAnh>();

    [ForeignKey("MaNhaCungCap")]
    [InverseProperty("DienThoais")]
    public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }
}
