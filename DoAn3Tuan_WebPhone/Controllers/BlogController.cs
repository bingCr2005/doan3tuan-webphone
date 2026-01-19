using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class BlogController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public BlogController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // Action hiển thị danh sách bài viết
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            // Truy vấn lấy danh sách bài viết và nạp thông tin danh mục
            var query = _context.BaiViets
                                .Include(b => b.MaDanhMucBvNavigation)
                                .Where(b => b.TrangThai == 1) // Chỉ lấy bài viết đang hoạt động
                                .OrderByDescending(b => b.NgayDang); // Sắp xếp bài mới nhất lên đầu

            // Xử lý tìm kiếm theo tiêu đề hoặc tóm tắt
            if (!string.IsNullOrEmpty(searchString))
            {
                query = (IOrderedQueryable<BaiViet>)query.Where(s => s.TieuDe.Contains(searchString) || s.TomTat.Contains(searchString));
            }

            // Cấu hình phân trang
            int pageSize = 5; // Số bài viết trên một trang
            var totalPosts = await query.CountAsync();
            var posts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Truyền dữ liệu phân trang ra View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            ViewBag.SearchString = searchString;

            // Lấy danh sách chuyên mục để hiển thị Sidebar (kèm số lượng bài viết)
            ViewBag.Categories = await _context.DanhMucBaiViets
                                               .Include(c => c.BaiViets)
                                               .ToListAsync();

            return View(posts);
        }

        // Action xem chi tiết bài viết
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            // Lấy thông tin bài viết chi tiết, bao gồm danh mục và người viết
            var baiViet = await _context.BaiViets
                .Include(b => b.MaDanhMucBvNavigation)
                .Include(b => b.MaNguoiVietNavigation)
                    .ThenInclude(admin => admin.MaAdminNavigation) // Lấy thông tin admin từ bảng TaiKhoan
                .FirstOrDefaultAsync(m => m.MaBaiViet == id);

            if (baiViet == null) return NotFound();

            // Tăng lượt xem cho bài viết
            baiViet.LuotXem = (baiViet.LuotXem ?? 0) + 1;
            _context.Update(baiViet);
            await _context.SaveChangesAsync();

            // Lấy danh sách bài viết liên quan (cùng danh mục, trừ bài hiện tại)
            ViewBag.RelatedPosts = await _context.BaiViets
                .Where(b => b.MaDanhMucBv == baiViet.MaDanhMucBv && b.MaBaiViet != id)
                .OrderByDescending(b => b.NgayDang)
                .Take(3)
                .ToListAsync();

            return View(baiViet);
        }
    }
}