using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class LienHe
{
    public int MaLienHe { get; set; }

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string NoiDung { get; set; } = null!;

    public DateTime? NgayGui { get; set; }

    public bool? TrangThai { get; set; }
}
