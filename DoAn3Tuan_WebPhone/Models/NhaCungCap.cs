using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class NhaCungCap
{
    public string MaNhaCungCap { get; set; } = null!;

    public string TenNhaCungCap { get; set; } = null!;

    public string? NguoiDaiDien { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public int? TrangThai { get; set; }

    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
