using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("KhuyenMai")]
public partial class KhuyenMai
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaKhuyenMai { get; set; } = null!;

    [StringLength(200)]
    public string? TenChuongTrinhKhuyenMai { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? MucGiamGia { get; set; }

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayHetHan { get; set; }

    public string? DieuKienApDung { get; set; }

    public int? SoLuongMaKhuyenMaiDaDung { get; set; }

    public int? TrangThai { get; set; }

    [InverseProperty("MaKhuyenMaiNavigation")]
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    [InverseProperty("MaKhuyenMaiNavigation")]
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
