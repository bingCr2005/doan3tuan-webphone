using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class HangDienThoai
{
    public string MaHangDienThoai { get; set; } = null!;

    public string TenHangDienThoai { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? TrangThai { get; set; }

    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
