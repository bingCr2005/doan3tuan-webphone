using Microsoft.AspNetCore.Mvc;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        // Hàm xử lý gửi form (POST)
        [HttpPost]
        public IActionResult GuiTin(string HoTen, string Email, string SoDienThoai, string NoiDung)
        {
            return RedirectToAction("Index");
        }
    }
}
