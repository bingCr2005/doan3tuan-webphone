using Microsoft.AspNetCore.Mvc;
using DoAn3Tuan_WebPhone.Models;
using System.Linq;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class LienHeController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public LienHeController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách liên hệ
        public IActionResult Index(string? search, int page = 1)
        {
            int pageSize = 5;

            // Lấy danh sách liên hệ
            var query = _context.LienHes.AsQueryable();

            // Nếu có tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(lh => lh.HoTen.Contains(search) || lh.Email.Contains(search));
            }

            // Tổng số trang
            int totalItems = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Search = search;

            // Lấy dữ liệu 5 dòng mỗi trang
            var dsLienHe = query
                .OrderByDescending(lh => lh.NgayGui)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(dsLienHe);
        }


        // Xem chi tiết (tùy chọn)
        public IActionResult Details(int id)
        {
            var lienHe = _context.LienHes.FirstOrDefault(x => x.MaLienHe == id);
            if (lienHe == null)
                return NotFound();

            lienHe.TrangThai = true;
            _context.SaveChanges();

            return View(lienHe);
        }

        // Xóa liên hệ
        public IActionResult Delete(int id)
        {
            var lienHe = _context.LienHes.Find(id);
            if (lienHe != null)
            {
                _context.LienHes.Remove(lienHe);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
