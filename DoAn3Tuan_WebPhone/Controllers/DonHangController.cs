using Microsoft.AspNetCore.Mvc;
using DoAn3Tuan_WebPhone.Models;
using Microsoft.EntityFrameworkCore;

public class DonHangController : Controller
{
    private readonly DBBanDienThoaiContext _context;

    public DonHangController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    //  DANH SÁCH ĐƠN HÀNG 
    public IActionResult Index(int? trangThai)
    {
        string maKH = "KH003";//HttpContext.Session.GetString("MaKH");
        if (maKH == null)
            return RedirectToAction("Login", "Account");

        var donHangs = _context.HoaDons
            .Where(x => x.MaKhachHang == maKH);

        if (trangThai != null)
            donHangs = donHangs.Where(x => x.TrangThai == trangThai);

        return View(donHangs
            .OrderByDescending(x => x.NgayLap)
            .ToList());
    }

    //  CHI TIẾT ĐƠN HÀNG 
    public IActionResult Detail(string id)
    {
        string maKH = "KH003";// HttpContext.Session.GetString("MaKH");
        if (maKH == null)
            return RedirectToAction("Login", "Account");

        var hoaDon = _context.HoaDons
            .Include(x => x.ChiTietHoaDons)
                .ThenInclude(ct => ct.MaDienThoaiNavigation)
            .FirstOrDefault(x =>
                x.MaHoaDon == id &&
                x.MaKhachHang == maKH);

        if (hoaDon == null)
            return NotFound();

        return View(hoaDon);
    }

    //  HỦY ĐƠN 
    [HttpPost]
    public IActionResult Cancel(string id)
    {
        string maKH = "KH003";// HttpContext.Session.GetString("MaKH");
        if (maKH == null)
            return RedirectToAction("Login", "Account");

        var don = _context.HoaDons
            .FirstOrDefault(x =>
                x.MaHoaDon == id &&
                x.MaKhachHang == maKH &&
                x.TrangThai == 0); // chỉ hủy khi chưa xử lý

        if (don != null)
        {
            don.TrangThai = -1; // -1 = đã hủy
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}
