using Microsoft.AspNetCore.Mvc;
using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Http;

namespace DoAn3Tuan_WebPhone.Controllers.Admin
{
    public class AdminLoginController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public AdminLoginController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // 1. Cập nhật đường dẫn View cho hàm GET
        public IActionResult Index()
        {
            // Chỉ định đường dẫn chính xác đến thư mục Admin/AdminLogin
            return View("~/Views/Admin/AdminLogin/Index.cshtml");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Kiểm tra tài khoản có tiền tố AD (Admin)
            var admin = _context.TaiKhoans
                .FirstOrDefault(x => x.TenDangNhap == username && x.MatKhau == password && x.MaTaiKhoan.StartsWith("AD"));

            if (admin != null)
            {
                // Lưu quyền Admin vào Session
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("TenUser", admin.HoVaTen);

                // Đăng nhập xong vào thẳng trang quản lý sản phẩm
                return RedirectToAction("Index", "AdminProduct");
            }

            // 2. Cập nhật đường dẫn View khi đăng nhập thất bại
            ViewBag.Error = "Thông tin đăng nhập Admin không chính xác!";
            return View("~/Views/Admin/AdminLogin/Index.cshtml");
        }

        public IActionResult Logout()
        {
            // Xóa Session và quay về trang đăng nhập của Admin
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}