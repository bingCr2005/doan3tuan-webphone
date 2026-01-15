using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("TaiKhoanKhachHang")]
public partial class TaiKhoanKhachHang
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaKhachHang { get; set; } = null!;

    [InverseProperty("MaKhachHangNavigation")]
    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    [InverseProperty("MaKhachHangNavigation")]
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    [InverseProperty("MaKhachHangNavigation")]
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [ForeignKey("MaKhachHang")]
    [InverseProperty("TaiKhoanKhachHang")]
    public virtual TaiKhoan MaKhachHangNavigation { get; set; } = null!;
}
