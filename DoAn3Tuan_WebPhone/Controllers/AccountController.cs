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

    // LOGIN - Hiển thị trang đăng nhập (nếu cần)
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var tk = _context.TaiKhoans
            .Include(x => x.TaiKhoanKhachHang)
            .FirstOrDefault(x => x.TenDangNhap == username && x.MatKhau == password);

        if (tk == null || tk.TaiKhoanKhachHang == null)
        {
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        // Lưu thông tin vào Session để dùng cho toàn hệ thống
        HttpContext.Session.SetString("MaKH", tk.TaiKhoanKhachHang.MaKhachHang);
        HttpContext.Session.SetString("TenUser", tk.HoVaTen);

        return RedirectToAction("Index", "Home");
    }

    // REGISTER - Thêm tài khoản mới vào Database
    [HttpPost]
    public async Task<IActionResult> Register(string hoten, string username, string email, string password)
    {
        // 1. Tạo mã KH mới tự động (Ví dụ: KH005)
        var count = await _context.TaiKhoans.CountAsync();
        string newId = "KH" + (count + 1).ToString("D3");

        // 2. Thêm vào bảng TaiKhoan
        var account = new TaiKhoan
        {
            MaTaiKhoan = newId,
            TenDangNhap = username,
            MatKhau = password,
            HoVaTen = hoten,
            Email = email,
            LoaiTaiKhoan = "KhachHang",
            TrangThai = "Hoạt động"
        };
        _context.TaiKhoans.Add(account);

        // 3. Thêm vào bảng TaiKhoanKhachHang
        _context.TaiKhoanKhachHangs.Add(new TaiKhoanKhachHang { MaKhachHang = newId });

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }

    // PROFILE - Sửa lỗi không còn bị cố định KH003
    public IActionResult Profile()
    {
        string maKH = "KH003";//HttpContext.Session.GetString("MaKH");

        if (string.IsNullOrEmpty(maKH))
            return RedirectToAction("Index", "Home");

        var taiKhoan = _context.TaiKhoans
            .Include(x => x.TaiKhoanKhachHang)
            .FirstOrDefault(x => x.TaiKhoanKhachHang.MaKhachHang == maKH);

        return View(taiKhoan);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}