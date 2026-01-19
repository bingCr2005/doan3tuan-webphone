using Microsoft.AspNetCore.Mvc;
using DoAn3Tuan_WebPhone.Models;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class ContactController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public ContactController(DBBanDienThoaiContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GuiTin(string HoTen, string Email, string SoDienThoai, string NoiDung)
        {
            if (ModelState.IsValid)// Kiểm tra dữ liệu hợp lệ
            {
                var lh = new LienHe
                {
                    HoTen = HoTen,      
                    Email = Email,       
                    SoDienThoai = SoDienThoai,
                    NoiDung = NoiDung,   
                    NgayGui = DateTime.Now, // Lưu ngày giờ gửi
                    TrangThai = 0         // 0: Tin nhắn mới (Chưa xử lý)
                };

                // Lưu vào bảng LienHes trong Database
                _context.LienHes.Add(lh);
                _context.SaveChanges();

                // Tạo thông báo Success
                TempData["Success"] = "Cảm ơn bạn đã liên hệ với chúng tôi! Chúng tôi sẽ liên lạc lại với bạn trong thời gian ngắn nhất.";

                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}