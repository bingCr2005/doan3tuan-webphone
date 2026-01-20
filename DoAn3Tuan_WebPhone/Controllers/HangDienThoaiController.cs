using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class HangDienThoaiController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public HangDienThoaiController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // 39. Tìm kiếm và Liệt kê hãng
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int pageSize = 10;
            var query = _context.HangDienThoais.AsQueryable();

            // 1. Kiểm tra từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(h => h.TenHangDienThoai.StartsWith(search));
            }

            // 2. Tính toán phân trang đồng bộ với giao diện bạn
            int totalItems = await query.CountAsync();
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Search = search; // Giữ lại từ khóa trên ô tìm kiếm

            var result = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(result);
        }

        // 36. Hiển thị trang Thêm hãng mới
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HangDienThoai hang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hang);
        }

        // 37. Hiển thị trang Cập nhật hãng
        public async Task<IActionResult> Edit(string id)
        {
            var hang = await _context.HangDienThoais.FindAsync(id);
            if (hang == null) return NotFound();
            return View(hang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HangDienThoai hang)
        {
            if (ModelState.IsValid)
            {
                _context.Update(hang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hang);
        }

        // 38. Xóa hãng
        // Hàm POST để thực hiện xóa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null) return NotFound();

            // Tìm và xóa hãng điện thoại
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
