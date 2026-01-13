using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Models;

[Table("TaiKhoanAdmin")]
public partial class TaiKhoanAdmin
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaAdmin { get; set; } = null!;

    [InverseProperty("MaAdminNavigation")]
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [ForeignKey("MaAdmin")]
    [InverseProperty("TaiKhoanAdmin")]
    public virtual TaiKhoan MaAdminNavigation { get; set; } = null!;
}
