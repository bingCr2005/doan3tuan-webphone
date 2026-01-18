using System.Collections.Generic;

namespace DoAn3Tuan_WebPhone.Models
{
    public class ChiTietSanPham
    {
        // 🔹 Thông tin sản phẩm chính
        public DienThoai DienThoai { get; set; } = null!;

        // 🔹 Danh sách hình ảnh
        public List<HinhAnh> HinhAnhs { get; set; } = new();

        // 🔹 Danh sách bình luận
        public List<BinhLuan> BinhLuans { get; set; } = new();

        // 🔹 Rating
        public double DiemTrungBinh { get; set; }
        public int TongSoBinhLuan { get; set; }

        // 🔹 Thống kê
        public int SoLuongYeuThich { get; set; }
        public int LuotXem { get; set; }

        // ✅ SẢN PHẨM LIÊN QUAN (CÙNG DANH MỤC)
        public List<DienThoai> SanPhamLienQuan { get; set; } = new();
    }
}
