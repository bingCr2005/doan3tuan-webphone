using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("HinhAnh")]
public partial class HinhAnh
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaHinhAnh { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string? MaDienThoai { get; set; }

    [StringLength(500)]
    public string? Url { get; set; }

    [ForeignKey("MaDienThoai")]
    [InverseProperty("HinhAnhs")]
    public virtual DienThoai? MaDienThoaiNavigation { get; set; }
}
