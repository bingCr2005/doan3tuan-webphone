
using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Area("Admin")]
public class AdminCommentController : Controller
{
    private readonly DBBanDienThoaiContext _context;

    public AdminCommentController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    // 1. Hiển thị danh sách bình luận (Đổ dữ liệu từ SQL vào Table)
    public async Task<IActionResult> Index(string searchString)
    {
        var query = _context.BinhLuans
            .Include(b => b.MaDienThoaiNavigation)
            .Include(b => b.MaKhachHangNavigation)
            .AsNoTracking()
            .AsQueryable();

        // Tìm kiếm theo nội dung hoặc tên khách hàng nếu cần
        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(b => b.NoiDung.Contains(searchString) ||
                                     b.MaKhachHangNavigation.MaKhachHang.Contains(searchString));
        }

        var data = await query.OrderByDescending(b => b.NgayBinhLuan).ToListAsync();
        ViewBag.Search = searchString;

        return View("~/Views/Admin/AdminComment/Index.cshtml", data);
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