
using DoAn3Tuan_WebPhone.Controllers.Admin;
using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminCommentController : AdminBaseController
{
    private readonly DBBanDienThoaiContext _context;

    public AdminCommentController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    // 1. Hiển thị danh sách bình luận (Đổ dữ liệu từ SQL vào Table)
    // Hiển thị danh sách bình luận kèm phân trang
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        // Cấu hình số lượng bản ghi hiển thị trên mỗi trang
        int pageSize = 10;
        var query = _context.BinhLuans
            .Include(b => b.MaDienThoaiNavigation)
            .Include(b => b.MaKhachHangNavigation)
            .AsNoTracking()
            .AsQueryable();

        // Tìm kiếm theo nội dung hoặc mã khách hàng
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(b => b.NoiDung.Contains(searchString) ||
                                     b.MaKhachHangNavigation.MaKhachHang.Contains(searchString));
        }

        // Tính toán tổng số trang dựa trên dữ liệu thực tế
        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Đảm bảo số trang luôn hợp lệ
        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        // Truy vấn dữ liệu theo phân khúc của trang hiện tại
        var data = await query.OrderByDescending(b => b.NgayBinhLuan)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

        // Gửi thông tin phân trang ra giao diện
        ViewBag.Search = searchString;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View("~/Views/Admin/AdminComment/Index.cshtml", data);
    }
    // 2. MỚI: Hàm Duyệt/Ẩn bình luận thay thế cho việc "Sửa" nội dung khách
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> CapNhatTrangThai(string id, int trangThaiMoi)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "Mã không hợp lệ" });

            var bl = await _context.BinhLuans.FindAsync(id.Trim());
            if (bl == null)
                return Json(new { success = false, message = "Không tìm thấy bình luận" });

            bl.TrangThai = trangThaiMoi; // 1: Hiện, 0: Ẩn
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = (trangThaiMoi == 1 ? "Đã duyệt hiển thị!" : "Đã ẩn bình luận!") });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi: " + ex.Message });
        }
    }

    // 2. Xử lý xóa bình luận: bằng ajax
    [IgnoreAntiforgeryToken]
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "Mã không hợp lệ" });

            var bl = await _context.BinhLuans.FindAsync(id.Trim());

            if (bl != null)
            {
                _context.BinhLuans.Remove(bl);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã xóa bình luận thành công!" });
            }

            return Json(new { success = false, message = "Không tìm thấy bình luận" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
        }
    }
}