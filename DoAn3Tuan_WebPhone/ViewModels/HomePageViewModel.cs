using DoAn3Tuan_WebPhone.Models;
namespace DoAn3Tuan_WebPhone.ViewModels
{
    public class HomePageViewModel
    {
        // Slideshow hiển thị các sản phẩm nổi bật, khuyến mãi, quảng cáo
        public List<DienThoai> SanPhamNoiBat { get; set; } = new();

        //sản phẩm chính (Sản phẩm HOT)
        public List<DienThoai> SanPhamMoi { get; set; } = new();
        // san phẩm mới về
        public List<DienThoai> SanPhamMoiVe { get; set; } = new();
        public List<DienThoai> SanPhamBanChay { get; set; } = new();
        public List<DienThoai> SanPhamDaXem { get; set; } = new();

        // Danh sách các hãng hiện ở Sidebar và Brand Slider
        public List<HangDienThoai> DanhSachHang { get; set; } = new();
    }
}
