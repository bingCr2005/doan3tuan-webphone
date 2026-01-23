using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Controllers.Admin
{
    [Route("AdminChat")] // Đường dẫn đẹp cho Admin
    public class AdminChatController : AdminBaseController // Kế thừa để có bảo mật
    {
        private readonly DBBanDienThoaiContext _context;

        public AdminChatController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // Danh sách tin nhắn từ khung chat
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var tinNhans = await _context.TinNhanChats
                .OrderByDescending(x => x.NgayGui) // Tin mới lên đầu
                .ToListAsync();

            return View("~/Views/Admin/AdminChat/Index.cshtml", tinNhans);
        }

        // Đánh dấu đã xử lý/đã đọc
        [HttpPost]
        [Route("MarkAsRead/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var tn = await _context.TinNhanChats.FindAsync(id);
            if (tn != null)
            {
                tn.TrangThai = 1; // Đã xử lý
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Xóa tin nhắn
        [HttpPost]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tn = await _context.TinNhanChats.FindAsync(id);
            if (tn != null)
            {
                _context.TinNhanChats.Remove(tn);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}