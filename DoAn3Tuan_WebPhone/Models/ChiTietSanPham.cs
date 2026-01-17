using System.Collections.Generic;
using DoAn3Tuan_WebPhone.Models;

namespace DoAn3Tuan_WebPhone.Models
{
    public class ChiTietSanPham
    {
        // Thông tin sản phẩm
        public DienThoai DienThoai { get; set; }
        // Thông tin khách hàng
        public TaiKhoan TaiKhoan { get; set; }
        // Thông tin bình luận
        public BinhLuan BinhLuan { get; set; }

        // Danh sách hình ảnh liên quan
        public List<HinhAnh> HinhAnhs { get; set; }

        // Danh sách bình luận của khách hàng
        public List<BinhLuan> BinhLuans { get; set; }

        // Điểm rating trung bình (tính từ các bình luận)
        public double DiemTrungBinh { get; set; }

        // Tổng số lượt bình luận
        public int TongSoBinhLuan { get; set; }

        // Tổng số lượt yêu thích
        public int SoLuongYeuThich { get; set; }

        // Tổng số lượt xem
        public int LuotXem { get; set; }
    }
}
