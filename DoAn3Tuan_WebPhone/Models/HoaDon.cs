using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("HoaDon")]
public partial class HoaDon
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaHoaDon { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhachHang { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaAdmin { get; set; }

    [StringLength(50)]
    public string? PhuongThucThanhToan { get; set; }

    public DateOnly? NgayLap { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhuyenMai { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TongTien { get; set; }

    public int? TrangThai { get; set; }

    [InverseProperty("MaHoaDonNavigation")]
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    [ForeignKey("MaAdmin")]
    [InverseProperty("HoaDons")]
    public virtual TaiKhoanAdmin? MaAdminNavigation { get; set; }

    [ForeignKey("MaKhachHang")]
    [InverseProperty("HoaDons")]
    public virtual TaiKhoanKhachHang? MaKhachHangNavigation { get; set; }

    [ForeignKey("MaKhuyenMai")]
    [InverseProperty("HoaDons")]
    public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
}
