using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAn3Tuan_WebPhone.Models;

public partial class NhaCungCap
{
    public string MaNhaCungCap { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập tên nhà cung cấp")]
    public string TenNhaCungCap { get; set; } = null!;

    public string? NguoiDaiDien { get; set; }

    [StringLength(20, ErrorMessage = "Số điện thoại không quá 20 ký tự")]
    public string? SoDienThoai { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ (ví dụ: ten@vn.com)")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,}$",
        ErrorMessage = "Email phải đúng định dạng, ví dụ: ten@vn.com")]
    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public int? TrangThai { get; set; }

    public virtual ICollection<DienThoai> DienThoais { get; set; } = new List<DienThoai>();
}
