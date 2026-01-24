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
    public IActionResult Index(int page = 1, int? trangThai = null)
    {
        string maKH = HttpContext.Session.GetString("MaKH");
        if (string.IsNullOrEmpty(maKH))
            return RedirectToAction("Login", "Account");
        int pageSize = 15;

        var query = _context.HoaDons
            .Where(x => x.MaKhachHang == maKH);

        if (trangThai != null)
            query = query.Where(x => x.TrangThai == trangThai);

        int totalItems = query.Count();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var donHangs = query
      .OrderByDescending(x => x.NgayLap)      // ngày mới nhất
      .ThenByDescending(x => x.MaHoaDon)      // HD mới nhất (HD010 > HD009)
      .Skip((page - 1) * pageSize)
      .Take(pageSize)
      .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TrangThai = trangThai;

        return View(donHangs);
    }


    //  CHI TIẾT ĐƠN HÀNG 
    public IActionResult Detail(string id)
    {
        string maKH =HttpContext.Session.GetString("MaKH");
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
    public IActionResult Cancel(string id, int page = 1, int? trangThai = null)
    {
        string maKH = HttpContext.Session.GetString("MaKH");
        if (string.IsNullOrEmpty(maKH))
            return RedirectToAction("Login", "Account");

        var don = _context.HoaDons.FirstOrDefault(x =>
            x.MaHoaDon == id &&
            x.MaKhachHang == maKH &&
            x.TrangThai == 0);

        if (don != null)
        {
            don.TrangThai = -1;
            _context.SaveChanges();
        }

        return RedirectToAction("Index", new { page, trangThai });
    }

}
