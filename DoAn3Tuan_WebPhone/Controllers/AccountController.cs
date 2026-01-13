using Microsoft.AspNetCore.Mvc;
using DoAn3Tuan_WebPhone.Models;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly DBBanDienThoaiContext _context;

    public AccountController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var tk = _context.TaiKhoans
            .Include(x => x.TaiKhoanKhachHang)
            .FirstOrDefault(x => x.TenDangNhap == username
                              && x.MatKhau == password);

        if (tk == null)
        {
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        // ✅ LƯU SESSION ĐÚNG CHUẨN
        HttpContext.Session.SetString(
            "MaKH",
            tk.TaiKhoanKhachHang.MaKhachHang
        );

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
