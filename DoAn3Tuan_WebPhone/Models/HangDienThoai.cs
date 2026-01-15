using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("HangDienThoai")]
public partial class HangDienThoai
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaHangDienThoai { get; set; } = null!;

    [StringLength(100)]
    public string TenHangDienThoai { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? TrangThai { get; set; }

    [InverseProperty("HangDienThoaiNavigation")]
    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
