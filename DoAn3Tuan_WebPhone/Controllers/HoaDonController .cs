using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HoaDonController : Controller
{
    private readonly DBBanDienThoaiContext _context;

    public HoaDonController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    //  Danh sách đơn hàng
    public IActionResult Index(int? trangThai, int page = 1)
    {
        int pageSize = 15; // số đơn mỗi trang

        var query = _context.HoaDons.AsQueryable();

        //  LỌC THEO TRẠNG THÁI
        if (trangThai.HasValue)
        {
            query = query.Where(hd => hd.TrangThai == trangThai);
        }

        //  TỔNG SỐ ĐƠN
        int totalItems = query.Count();
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //  PHÂN TRANG
        var hoaDons = query
            .OrderByDescending(hd => hd.NgayLap)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // GỬI DỮ LIỆU RA VIEW
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TrangThai = trangThai;

        return View(hoaDons);
    }


    //  Form cập nhật
    public IActionResult Edit(string id)
    {
        var hoaDon = _context.HoaDons.FirstOrDefault(h => h.MaHoaDon == id);
        if (hoaDon == null) return NotFound();

        return View(hoaDon);
    }

    //  Xử lý cập nhật
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(HoaDon model)
    {
        var hoaDon = _context.HoaDons.FirstOrDefault(h => h.MaHoaDon == model.MaHoaDon);
        if (hoaDon == null) return NotFound();

        hoaDon.TrangThai = model.TrangThai;
        hoaDon.TenNguoiNhan = model.TenNguoiNhan;
        hoaDon.SoDienThoai = model.SoDienThoai;
        hoaDon.DiaChiGiaoHang = model.DiaChiGiaoHang;

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // Xóa đơn hàng
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(string id)
    {
        var hoaDon = _context.HoaDons.FirstOrDefault(h => h.MaHoaDon == id);
        if (hoaDon == null) return NotFound();

        hoaDon.TrangThai = -1;
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
