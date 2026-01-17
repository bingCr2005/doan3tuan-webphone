using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class TaiKhoanAdmin
{
    public string MaAdmin { get; set; } = null!;

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual TaiKhoan MaAdminNavigation { get; set; } = null!;
}
