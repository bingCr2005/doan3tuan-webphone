using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Controllers
{
  
    public class DienThoaiController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public DienThoaiController(DBBanDienThoaiContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string keyword, string brandId, decimal? minPrice, decimal? maxPrice, int page = 1)
        {
            int pageSize = 9; // Số sản phẩm trên mỗi trang

            // Khởi tạo truy vấn và nạp kèm bảng HinhAnh (Join) [cite: 3, 5]
            var query = _context.DienThoais.Include(p => p.HinhAnhs).Where(p => p.TrangThai == 1).AsQueryable();

            // 1. Tìm theo từ khóa (Tên hoặc Mô tả)
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.TenDienThoai.Contains(keyword) || p.MoTa.Contains(keyword));
            }

            // 2. Tìm theo danh mục (Hãng điện thoại)
            if (!string.IsNullOrEmpty(brandId))
            {
                query = query.Where(p => p.HangDienThoai == brandId);
            }

            // 3. Tìm theo khoảng giá
            if (minPrice.HasValue) query = query.Where(p => p.DonGia >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(p => p.DonGia <= maxPrice.Value);

            // Tính toán phân trang
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var products = await query
                .OrderByDescending(p => p.MaDienThoai)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new SearchViewModel
            {
                Products = products,
                Brands = await _context.HangDienThoais.Where(b => b.TrangThai == 1).ToListAsync(), // [cite: 2, 38]
                Keyword = keyword,
                BrandId = brandId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View("Search",model);
        }
    }
}
