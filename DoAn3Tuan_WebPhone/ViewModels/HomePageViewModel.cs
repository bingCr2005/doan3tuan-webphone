using DoAn3Tuan_WebPhone.Models;
namespace DoAn3Tuan_WebPhone.ViewModels
{
    public class HomePageViewModel
    {
        //Sản phẩm nổi bật hoặc đang giảm giá mạnh để chạy Slideshow
        public List<DienThoai> SanPhamNoiBat { get; set; } = new();

        //Danh sách các điện thoại mới nhập về
        public List<DienThoai> SanPhamMoi { get; set; } = new();

        //Danh sách điện thoại có số lượng bán ra nhiều nhất
        public List<DienThoai> SanPhamBanChay { get; set; } = new();

        // Danh sách các hãng hiện ở Sidebar và Brand Slider
        public List<HangDienThoai> DanhSachHang { get; set; } = new();
    }
}
