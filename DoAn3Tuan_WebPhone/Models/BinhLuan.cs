using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("BinhLuan")]
public partial class BinhLuan
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaBinhLuan { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? MaDienThoai { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhachHang { get; set; }

    public int? SoSao { get; set; }

    public string? NoiDung { get; set; }

    public DateOnly? NgayBinhLuan { get; set; }

    public int? TrangThai { get; set; }

    [ForeignKey("MaDienThoai")]
    [InverseProperty("BinhLuans")]
    public virtual DienThoai? MaDienThoaiNavigation { get; set; }

    [ForeignKey("MaKhachHang")]
    [InverseProperty("BinhLuans")]
    public virtual TaiKhoanKhachHang? MaKhachHangNavigation { get; set; }
}
