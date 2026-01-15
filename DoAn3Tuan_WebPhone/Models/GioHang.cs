using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("GioHang")]
public partial class GioHang
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaGioHang { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhachHang { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaDienThoai { get; set; }

    public int? SoLuongHang { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ThanhTien { get; set; }

    [ForeignKey("MaDienThoai")]
    [InverseProperty("GioHangs")]
    public virtual DienThoai? MaDienThoaiNavigation { get; set; }

    [ForeignKey("MaKhachHang")]
    [InverseProperty("GioHangs")]
    public virtual TaiKhoanKhachHang? MaKhachHangNavigation { get; set; }
}
