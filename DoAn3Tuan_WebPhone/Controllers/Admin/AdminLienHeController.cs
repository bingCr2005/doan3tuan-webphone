using Microsoft.AspNetCore.Mvc;
using DoAn3Tuan_WebPhone.Models;
using System.Linq;

namespace DoAn3Tuan_WebPhone.Controllers.Admin
{
    public class AdminLienHeController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public AdminLienHeController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách liên hệ
        public IActionResult Index(string? search, int page = 1)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            int pageSize = 5;

            // Lấy danh sách liên hệ
            var query = _context.LienHes.AsQueryable();

            // Tìm kiếm theo tên và email
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

            return View("~/Views/Admin/AdminLienHe/Index.cshtml",dsLienHe);
        }

        // Hiển thị form sửa
        public IActionResult Edit(int id)
        {
            var lienHe = _context.LienHes.Find(id);
            if (lienHe == null)
                return NotFound();

            return View("~/Views/Admin/AdminLienHe/Edit.cshtml",lienHe);
        }

        //Cập nhật liên hệ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LienHe model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var lienHe = _context.LienHes.Find(model.MaLienHe);
            if (lienHe == null)
                return NotFound();

            lienHe.HoTen = model.HoTen;
            lienHe.Email = model.Email;
            lienHe.SoDienThoai = model.SoDienThoai;
            lienHe.NoiDung = model.NoiDung;
            lienHe.TrangThai = model.TrangThai;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // Xóa liên hệ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
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
