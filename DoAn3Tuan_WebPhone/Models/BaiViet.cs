using System;
using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models;

public partial class BaiViet
{
    public string MaBaiViet { get; set; } = null!;

    public string TieuDe { get; set; } = null!;

    public string? TomTat { get; set; }

    public string? NoiDung { get; set; }

    public string? HinhAnh { get; set; }

    public DateOnly? NgayDang { get; set; }

    public int? LuotXem { get; set; }

    public int? MaDanhMucBv { get; set; }

    public string? MaNguoiViet { get; set; }

    public string? MaDienThoai { get; set; }

    public int? TrangThai { get; set; }

    public virtual DanhMucBaiViet? MaDanhMucBvNavigation { get; set; }

    public virtual DienThoai? MaDienThoaiNavigation { get; set; }

    public virtual TaiKhoanAdmin? MaNguoiVietNavigation { get; set; }
}
