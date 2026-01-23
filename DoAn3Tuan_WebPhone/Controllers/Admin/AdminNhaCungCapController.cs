using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Controllers.Admin
{
    public class AdminNhaCungCapController : Controller
    {
        private readonly DBBanDienThoaiContext _context;
        public AdminNhaCungCapController(DBBanDienThoaiContext context)
        {
            _context = context;
        }
        // Hiển thị danh sách nhà cung cấp
        public IActionResult Index(string? search, int page = 1)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "AdminLogin");
            }
            int pageSize = 5;

            // Lấy danh sách nhà cung cấp
            var query = _context.NhaCungCaps.AsQueryable();

            // Tìm kiếm theo tên nhà cung cấp và người đại diện
            if (!string.IsNullOrEmpty(search))
            {
                string keyword = search.ToLower();
                query = query.Where(ncc =>
                    (ncc.TenNhaCungCap != null && ncc.TenNhaCungCap.ToLower().Contains(keyword)) ||
                    (ncc.NguoiDaiDien != null && ncc.NguoiDaiDien.ToLower().Contains(keyword))
                );
            }

            // Tổng số trang
            int totalItems = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Search = search;

            // Lấy dữ liệu 5 dòng mỗi trang
            var dsNCC = query
                .OrderByDescending(ncc => ncc.MaNhaCungCap)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View("~/Views/Admin/AdminNhaCungCap/Index.cshtml",dsNCC);
        }

        // Hiển thị form cập nhật
        public IActionResult Edit(string id)
        {
            var nhaCungCap = _context.NhaCungCaps.Find(id);
            if (nhaCungCap == null)
                return NotFound();

            return View("~/Views/Admin/AdminNhaCungCap/Edit.cshtml",nhaCungCap);
        }

        //Cập nhật nhà cung cấp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NhaCungCap model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/AdminNhaCungCap/Edit.cshtml",model);

            var nhaCungCap = _context.NhaCungCaps.Find(model.MaNhaCungCap);
            if (nhaCungCap == null)
                return NotFound();

            nhaCungCap.TenNhaCungCap = model.TenNhaCungCap;
            nhaCungCap.NguoiDaiDien = model.NguoiDaiDien;
            nhaCungCap.SoDienThoai = model.SoDienThoai;
            nhaCungCap.Email = model.Email;
            nhaCungCap.DiaChi = model.DiaChi;
            nhaCungCap.TrangThai = model.TrangThai;

            _context.Update(nhaCungCap);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        //Hiển thị form tạo mới
        public IActionResult Create()
        {
            // Lấy mã cuối cùng trong DB
            var lastNCC = _context.NhaCungCaps
                .OrderByDescending(ncc => ncc.MaNhaCungCap)
                .FirstOrDefault();

            string newId = "NCC001"; // Mặc định nếu chưa có dữ liệu

            if (lastNCC != null)
            {
                // Cắt phần số từ mã cuối cùng (bỏ "NCC")
                string numberPart = lastNCC.MaNhaCungCap.Substring(3).Trim();
                if (int.TryParse(numberPart, out int number))
                {
                    newId = "NCC" + (number + 1).ToString("D3");
                }
            }

            // Truyền mã mới sang view
            ViewBag.NewId = newId;

            return View("~/Views/Admin/AdminNhaCungCap/Create.cshtml");
        }
        //Thêm nhà cung cấp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NhaCungCap model)
        {
            // Kiểm tra dữ liệu form có hợp lệ không
            if (!ModelState.IsValid)
                return View("~/Views/Admin/AdminNhaCungCap/Create.cshtml", model);

            // Sinh mã tự động nếu chưa có
            if (string.IsNullOrEmpty(model.MaNhaCungCap))
            {
                // Lấy nhà cung cấp có mã lớn nhất hiện tại
                var lastNCC = _context.NhaCungCaps
                    .OrderByDescending(ncc => ncc.MaNhaCungCap)
                    .FirstOrDefault();

                string newId = "NCC001"; // Mặc định nếu chưa có

                if (lastNCC != null)
                {
                    // Bỏ phần "NCC" để lấy phần số
                    string numberPart = lastNCC.MaNhaCungCap.Substring(3).Trim();

                    if (int.TryParse(numberPart, out int number))
                    {
                        int nextNumber = number + 1;

                        // Sinh mã mới luôn có 3 chữ số
                        newId = "NCC" + nextNumber.ToString("D3");
                    }
                }

                model.MaNhaCungCap = newId;
            }

            model.MaNhaCungCap = model.MaNhaCungCap.Trim();
            model.TenNhaCungCap = model.TenNhaCungCap.Trim();
            model.NguoiDaiDien = model.NguoiDaiDien?.Trim();
            model.SoDienThoai = model.SoDienThoai?.Trim();
            model.Email = model.Email?.Trim();
            model.DiaChi = model.DiaChi?.Trim();

            try
            {
                // Thêm vào DB
                _context.NhaCungCaps.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = $"Đã thêm nhà cung cấp {model.TenNhaCungCap} ({model.MaNhaCungCap}) thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Nếu lỗi, trả lại view với thông báo
                ModelState.AddModelError(string.Empty, "Lỗi khi lưu dữ liệu: " + ex.Message);
                return View("~/Views/Admin/AdminNhaCungCap/Create.cshtml", model);
            }
        }

        // Xóa nhà cung cấp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var nhaCungCap = _context.NhaCungCaps.Find(id);
            if (nhaCungCap != null)
            {
                _context.NhaCungCaps.Remove(nhaCungCap);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
