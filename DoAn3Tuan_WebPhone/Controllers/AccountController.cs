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


    //  LOGIN 
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var tk = _context.TaiKhoans
            .Include(x => x.TaiKhoanKhachHang)
            .FirstOrDefault(x =>
                x.TenDangNhap == username &&
                x.MatKhau == password);

        if (tk == null || tk.TaiKhoanKhachHang == null)
        {
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

            
        HttpContext.Session.SetString(
            "MaKH",
            tk.TaiKhoanKhachHang.MaKhachHang
        );

        return RedirectToAction("Index", "Home");
    }

    //  LOGOUT 
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
    // PROFILE
    public IActionResult Profile()
    {
        string maKH = "KH003";

        var taiKhoan = _context.TaiKhoans
            .Include(x => x.TaiKhoanKhachHang)
            .FirstOrDefault(x => x.TaiKhoanKhachHang.MaKhachHang == maKH);

        if (taiKhoan == null)
            return RedirectToAction("Login");

        return View(taiKhoan);
    }



}
