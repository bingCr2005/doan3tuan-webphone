using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Controllers.Admin
{
    // 1. Kế thừa AdminBaseController để dùng chung cấu hình bảo mật/layout
    public class AdminHangController : AdminBaseController
    {
        private readonly DBBanDienThoaiContext _context;

        public AdminHangController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // --- 1. LIỆT KÊ & TÌM KIẾM ---
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int pageSize = 10;
            var query = _context.HangDienThoais.AsNoTracking(); // Dùng AsNoTracking để tăng tốc độ load

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(h => h.TenHangDienThoai.Contains(search));
            }

            int totalItems = await query.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Search = search;

            var result = await query
                .OrderBy(h => h.MaHangDienThoai)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Chỉ định đường dẫn View thủ công vào folder con
            return View("~/Views/Admin/AdminHang/Index.cshtml", result);
        }

        // --- 2. THÊM MỚI ---
        public IActionResult Create() => View("~/Views/Admin/AdminHang/Create.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HangDienThoai hang)
        {
            // Loại bỏ kiểm tra danh sách điện thoại liên quan để ModelState hợp lệ
            ModelState.Remove("DienThoais");

            if (ModelState.IsValid)
            {
                _context.Add(hang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/AdminHang/Create.cshtml", hang);
        }

        // --- 3. CHỈNH SỬA ---
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            // Trim ID vì kiểu dữ liệu CHAR(10) trong SQL
            var hang = await _context.HangDienThoais.FindAsync(id.Trim());
            if (hang == null) return NotFound();

            return View("~/Views/Admin/AdminHang/Edit.cshtml", hang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HangDienThoai hang)
        {
            ModelState.Remove("DienThoais");

            if (ModelState.IsValid)
            {
                hang.MaHangDienThoai = hang.MaHangDienThoai.Trim();
                _context.Update(hang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/AdminHang/Edit.cshtml", hang);
        }

        // --- 4. XÓA ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var hangDienThoai = await _context.HangDienThoais.FindAsync(id.Trim());
            if (hangDienThoai != null)
            {
                _context.HangDienThoais.Remove(hangDienThoai);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}