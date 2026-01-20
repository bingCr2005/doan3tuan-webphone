using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;
using DoAn3Tuan_WebPhone.ViewModels;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public HomeController(DBBanDienThoaiContext context)
        {
            _context = context;
        }
        // gioi thieu ve congty 
        public IActionResult About()
        {
            return View(); //Views/Home/About.cshtml
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;
            var viewModel = new HomePageViewModel();

            // T?o base query ?? dùng chung
            var baseQuery = _context.DienThoais.Where(p => p.TrangThai == 1);

            // 1. L?y d? li?u cho Slideshow (Ch? l?y nh?ng gì c?n thi?t)
            var topViews = await baseQuery.OrderByDescending(p => p.LuotXem).Take(5).ToListAsync();

            viewModel.SanPhamNoiBat = new List<DienThoai>();
            // Logic: L?y con hot nh?t, con r? nh?t và con iPhone 17 t? list topViews ho?c DB
            viewModel.SanPhamNoiBat.Add(topViews.First());
            viewModel.SanPhamNoiBat.Add(await baseQuery.OrderBy(p => p.DonGia).FirstOrDefaultAsync());
            viewModel.SanPhamNoiBat.Add(await baseQuery.FirstOrDefaultAsync(p => p.TenDienThoai.Contains("iPhone 17")));
            // Lo?i b? các ph?n t? null n?u không tìm th?y máy
            viewModel.SanPhamNoiBat.RemoveAll(item => item == null);

            // 2. Phân trang t?i ?u
            int totalItems = await baseQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            page = Math.Clamp(page, 1, totalPages > 0 ? totalPages : 1);

            viewModel.SanPhamMoi = await baseQuery
                .Include(p => p.HangDienThoaiNavigation)
                .OrderByDescending(p => p.MaDienThoai) // S?n ph?m M?I thì nên theo Mã ho?c Ngày t?o
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            viewModel.DanhSachHang = await _context.HangDienThoais.ToListAsync();

            return View(viewModel);
        }
    }
}