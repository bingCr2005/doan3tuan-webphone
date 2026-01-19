using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class DanhMucBaiViet
{
    public int MaDanhMucBv { get; set; }

    public string TenDanhMuc { get; set; } = null!;

    public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
}
