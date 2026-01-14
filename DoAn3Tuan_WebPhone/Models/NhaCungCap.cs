using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("NhaCungCap")]
public partial class NhaCungCap
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNhaCungCap { get; set; } = null!;

    [StringLength(100)]
    public string TenNhaCungCap { get; set; } = null!;

    [StringLength(100)]
    public string? NguoiDaiDien { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? SoDienThoai { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public int? TrangThai { get; set; }

    [InverseProperty("MaNhaCungCapNavigation")]
    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
