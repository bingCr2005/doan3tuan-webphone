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
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.HangDienThoais.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(h => h.TenHangDienThoai.Contains(searchString));
            }

            return View(await query.ToListAsync());
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
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var hang = await _context.HangDienThoais.FindAsync(id);
            if (hang != null)
            {
                _context.HangDienThoais.Remove(hang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
