using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class ProductController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public ProductController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // Action Index xử lý Tìm kiếm, Lọc theo Hãng và Phân trang
        public async Task<IActionResult> Index(string hangId, string searchString, int page = 1)
        {
            int pageSize = 10; // Mỗi trang hiện 10 sản phẩm
            var query = _context.DienThoais.Include(p => p.HangDienThoaiNavigation).AsQueryable();

            // 1. Lọc theo hãng nếu có hangId truyền từ Home qua
            if (!string.IsNullOrEmpty(hangId))
            {
                query = query.Where(p => p.HangDienThoai == hangId);
            }

            // 2. Tìm kiếm theo tên nếu có searchString
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.TenDienThoai.Contains(searchString));
            }

            // 3. Phân trang
            int totalItems = await query.CountAsync();
            var danhSach = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Truyền dữ liệu bổ sung ra View qua ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentHang = hangId;
            ViewBag.CurrentSearch = searchString;

            return View(danhSach);
        }
    }
}