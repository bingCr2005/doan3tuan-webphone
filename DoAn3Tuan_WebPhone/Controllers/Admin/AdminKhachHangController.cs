using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Controllers.Admin // Namespace chuẩn folder con
{
    public class AdminKhachHangController : AdminBaseController // Kế thừa bảo mật
    {
        private readonly DBBanDienThoaiContext _context;

        public AdminKhachHangController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // --- 1. LIỆT KÊ & TÌM KIẾM ---
        public async Task<IActionResult> Index(string searchString)
        {
            var query = _context.TaiKhoans.Where(t => t.LoaiTaiKhoan == "KhachHang").AsNoTracking();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(t => t.HoVaTen.Contains(searchString) || t.Email.Contains(searchString));
            }

            var data = await query.OrderByDescending(t => t.NgayTao).ToListAsync();
            ViewBag.Search = searchString;
            return View("~/Views/Admin/AdminKhachHang/Index.cshtml", data); // Đường dẫn View thủ công
        }

        // --- 2. THÊM MỚI (CREATE) ---
        public IActionResult Create() => View("~/Views/Admin/AdminKhachHang/Create.cshtml");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                model.LoaiTaiKhoan = "KhachHang";
                model.TrangThai = "Hoạt động"; // Dữ liệu NVARCHAR theo DB mày

                _context.TaiKhoans.Add(model);
                await _context.SaveChangesAsync();

                // Phải thêm vào bảng TaiKhoanKhachHang để đồng bộ DB
                var kh = new TaiKhoanKhachHang { MaKhachHang = model.MaTaiKhoan };
                _context.TaiKhoanKhachHangs.Add(kh);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/AdminKhachHang/Create.cshtml", model);
        }

        // --- 3. CHỈNH SỬA (EDIT) ---
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _context.TaiKhoans.FindAsync(id);
            if (user == null) return NotFound();
            return View("~/Views/Admin/AdminKhachHang/Edit.cshtml", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaiKhoan model)
        {
            // 1. Loại bỏ kiểm tra các trường KHÔNG có trong Form Edit để ModelState.IsValid luôn True
            ModelState.Remove("MatKhau");
            ModelState.Remove("TenDangNhap");
            ModelState.Remove("LoaiTaiKhoan");
            ModelState.Remove("NgayTao");
            ModelState.Remove("TaiKhoanAdmin");
            ModelState.Remove("TaiKhoanKhachHang");
            ModelState.Remove("BinhLuans");
            ModelState.Remove("GioHangOlds");

            if (ModelState.IsValid)
            {
                try
                {
                    // 2. Tìm bản ghi hiện tại trong DB để lấy lại các thông tin không sửa (Mật khẩu, Loại TK...)
                    var existingUser = await _context.TaiKhoans.AsNoTracking()
                        .FirstOrDefaultAsync(t => t.MaTaiKhoan == model.MaTaiKhoan.Trim());

                    if (existingUser != null)
                    {
                        // Gán lại các giá trị quan trọng từ DB để tránh bị Null khi Update
                        model.MatKhau = existingUser.MatKhau;
                        model.TenDangNhap = existingUser.TenDangNhap;
                        model.LoaiTaiKhoan = existingUser.LoaiTaiKhoan;
                        model.NgayTao = existingUser.NgayTao;
                        model.MaTaiKhoan = model.MaTaiKhoan.Trim();

                        _context.Update(model);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Lỗi xung đột dữ liệu, vui lòng thử lại.");
                }
            }

            // 3. Nếu vẫn không được, in lỗi ra cửa sổ Output để mày "soi"
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                System.Diagnostics.Debug.WriteLine("LỖI MODELSTATE: " + error.ErrorMessage);
            }

            return View("~/Views/Admin/AdminKhachHang/Edit.cshtml", model);
        }

        // --- 4. XÓA (DELETE) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var kh = await _context.TaiKhoanKhachHangs.FindAsync(id);
            if (kh != null) _context.TaiKhoanKhachHangs.Remove(kh); // Xóa bảng phụ trước

            var tk = await _context.TaiKhoans.FindAsync(id);
            if (tk != null) _context.TaiKhoans.Remove(tk);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var maKH = id.Trim();
            // 1. Lấy thông tin tài khoản 
            var khachHang = await _context.TaiKhoans
                .FirstOrDefaultAsync(m => m.MaTaiKhoan == maKH);

            if (khachHang == null) return NotFound();

            // 2. Lấy lịch sử hóa đơn và chi tiết máy đã mua [cite: 12, 13, 39]
            ViewBag.HoaDons = await _context.HoaDons
                .Include(h => h.ChiTietHoaDons)
                .ThenInclude(ct => ct.MaDienThoaiNavigation)
                .Where(h => h.MaKhachHang == maKH)
                .OrderByDescending(h => h.NgayLap)
                .ToListAsync();
            // 3. Lấy giỏ hàng hiện tại [cite: 62, 63, 39]
            ViewBag.GioHang = await _context.ChiTietGioHangs
                .Include(ct => ct.MaDienThoaiNavigation)
                .Where(ct => ct.MaGioHangNavigation.MaKhachHang == maKH)
                .ToListAsync();

            // 4. Lấy các bình luận đã viết [cite: 9, 39]
            ViewBag.BinhLuans = await _context.BinhLuans
                .Include(b => b.MaDienThoaiNavigation)
                .Where(b => b.MaKhachHang == maKH)
                .OrderByDescending(b => b.NgayBinhLuan)
                .ToListAsync();

            return View("~/Views/Admin/AdminKhachHang/Details.cshtml", khachHang);
        }
    }
}