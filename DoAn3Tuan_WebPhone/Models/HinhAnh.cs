using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class HinhAnh
{
    public string MaHinhAnh { get; set; } = null!;

    public string? MaDienThoai { get; set; }

    public string? Url { get; set; }

    public virtual DienThoai? MaDienThoaiNavigation { get; set; }
}
