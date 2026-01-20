using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models; // Thay bằng namespace thực tế của bạn

public class CartSummaryViewComponent : ViewComponent
{
    private readonly DBBanDienThoaiContext _context;

    public CartSummaryViewComponent(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // 1. Lấy thông tin tài khoản đang đăng nhập
        var userName = User.Identity.Name;
        var khachHang = await _context.TaiKhoans.FirstOrDefaultAsync(u => u.TenDangNhap == userName);

        var model = new TongGioHang(); // Chứa TongSoLuongSP và TongTien

        if (khachHang != null)
        {
            // 2. Tìm mã giỏ hàng dựa trên MaKhachHang
            var gioHang = await _context.GioHangs
                .FirstOrDefaultAsync(g => g.MaKhachHang == khachHang.MaTaiKhoan);

            if (gioHang != null)
            {
                // 3. Truy vấn trực tiếp vào bảng ChiTietGioHang qua MaGioHang
                var cartItems = _context.ChiTietGioHangs
                    .Where(c => c.MaGioHang == gioHang.MaGioHang);

                // Sửa lỗi CS0266 bằng cách xử lý giá trị Null (?? 0)
                // Đảm bảo đáp ứng Mục 2: Tổng tiền và số lượng
                model.TongSoLuongSP = (await cartItems.SumAsync(i => i.SoLuong)) ?? 0;
                model.TongTien = (await cartItems.SumAsync(i => i.ThanhTien)) ?? 0;
            }
        }

        return View(model);
    }
}