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
        // 1. Tìm tài khoản khớp User/Pass và nạp bảng Khách hàng nếu có
        var tk = _context.TaiKhoans
            .Include(x => x.TaiKhoanKhachHang)
            .FirstOrDefault(x => x.TenDangNhap == username && x.MatKhau == password);

        if (tk == null)
        {
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        // 2. Lưu thông tin cơ bản vào Session
        HttpContext.Session.SetString("MaTK", tk.MaTaiKhoan);
        HttpContext.Session.SetString("TenUser", tk.HoVaTen);

        // 3. Phân quyền dựa trên mã ID hoặc thuộc tính LoaiTaiKhoan
        if (tk.MaTaiKhoan.StartsWith("AD") || tk.LoaiTaiKhoan == "Admin")
        {
            // TRƯỜNG HỢP: ADMIN / NHÂN VIÊN
            HttpContext.Session.SetString("Role", "Admin");
            // Chuyển hướng đến trang quản trị (Area Admin nếu có)
            return RedirectToAction("Index", "AdminProduct", new { area = "Admin" });
        }
        else
        {
            // TRƯỜNG HỢP: KHÁCH HÀNG
            HttpContext.Session.SetString("Role", "Customer");
            if (tk.TaiKhoanKhachHang != null)
            {
                HttpContext.Session.SetString("MaKH", tk.TaiKhoanKhachHang.MaKhachHang);
            }
            return RedirectToAction("Index", "Home");
        }
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
        string maKH = HttpContext.Session.GetString("MaKH");

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