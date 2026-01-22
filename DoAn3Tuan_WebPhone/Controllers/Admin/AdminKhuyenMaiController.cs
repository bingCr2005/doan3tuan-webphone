using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Controllers.Admin
{
    public class AdminKhuyenMaiController : AdminBaseController
    {
        private readonly DBBanDienThoaiContext _context;

        public AdminKhuyenMaiController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // Truy xuất danh sách chương trình khuyến mãi kèm phân trang
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            // Cấu hình số lượng bản ghi hiển thị trên mỗi trang
            int pageSize = 10;
            var query = _context.KhuyenMais.AsNoTracking().AsQueryable();

            // Lọc dữ liệu theo tên chương trình hoặc mã voucher
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(k => k.TenChuongTrinhKhuyenMai.Contains(searchString) ||
                                         k.MaKhuyenMai.Contains(searchString));
            }

            // Tính toán tổng số trang dựa trên dữ liệu lọc được
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Ràng buộc số trang nằm trong phạm vi hợp lệ
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            // Lấy dữ liệu theo vị trí trang hiện tại
            var data = await query.OrderByDescending(k => k.NgayBatDau)
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();

            // Chuyển các thông số phân trang sang View
            ViewBag.Search = searchString;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View("~/Views/Admin/AdminKhuyenMai/Index.cshtml", data);
        }

        // Hiển thị giao diện tạo mới khuyến mãi
        public IActionResult Create()
        {
            return View("~/Views/Admin/AdminKhuyenMai/Create.cshtml");
        }

        // Tiếp nhận và lưu trữ thông tin khuyến mãi mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuyenMai km)
        {
            if (ModelState.IsValid)
            {
                // Khởi tạo giá trị mặc định cho số lượng sử dụng
                km.SoLuongMaKhuyenMaiDaDung = 0;
                _context.Add(km);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/AdminKhuyenMai/Create.cshtml", km);
        }

        // Hiển thị giao diện chỉnh sửa khuyến mãi dựa trên mã ID
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            // Loại bỏ khoảng trắng khi tìm kiếm khóa chính kiểu CHAR
            var km = await _context.KhuyenMais.FirstOrDefaultAsync(x => x.MaKhuyenMai.Trim() == id.Trim());

            if (km == null) return NotFound();
            return View("~/Views/Admin/AdminKhuyenMai/Edit.cshtml", km);
        }

        // Cập nhật các thay đổi của chương trình khuyến mãi vào cơ sở dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KhuyenMai km)
        {
            if (ModelState.IsValid)
            {
                _context.Update(km);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Admin/AdminKhuyenMai/Edit.cshtml", km);
        }

        // Thực hiện xóa chương trình khuyến mãi khỏi hệ thống
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            // Tìm kiếm bản ghi chính xác trước khi thực hiện xóa
            var km = await _context.KhuyenMais.FirstOrDefaultAsync(x => x.MaKhuyenMai.Trim() == id.Trim());
            if (km != null)
            {
                _context.KhuyenMais.Remove(km);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}