using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

public class ThanhToanController : Controller
{
    private readonly DbbanDienThoaiContext _context;

    public ThanhToanController(DbbanDienThoaiContext context)
    {
        _context = context;
    }

    // GET: /ThanhToan/Checkout
    public IActionResult Checkout()
    {
        string maKH = "KH003"; // Test

        /*HttpContext.Session.GetString("MaKH");
        if (maKH == null)
            return RedirectToAction("Login", "Account"); */ //test khi chưa có chức năng đăng nhập

        var gioHang = _context.GioHangs
    .Include(g => g.ChiTietGioHangs)
        .ThenInclude(ct => ct.MaDienThoaiNavigation)
    .FirstOrDefault(g => g.MaKhachHang == maKH);

        if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            return RedirectToAction("Index", "GioHang");

        return View(gioHang.ChiTietGioHangs.ToList());
    }

    // POST: /ThanhToan/Checkout
    [HttpPost]
    public IActionResult Confirm(
    List<int> ctghIds,
    string tenNguoiNhan,
    string sdt,
    string diaChi,
    string phuongThuc)
    {
        string maKH = "KH003"; // test

        // VALIDATE
        if (string.IsNullOrWhiteSpace(tenNguoiNhan) ||
            string.IsNullOrWhiteSpace(sdt) ||
            string.IsNullOrWhiteSpace(diaChi))
        {
            TempData["Error"] = "Vui lòng nhập đầy đủ thông tin người nhận";
            return RedirectToAction("Checkout");
        }
        // CHECK TÊN 
        if (!Regex.IsMatch(tenNguoiNhan, @"^[A-Za-zÀ-ỹ\s]+$"))
        {
            TempData["Error"] = "Tên người nhận không được chứa số";
            return RedirectToAction("Checkout");
        }

        // CHECK SĐT 10 SỐ
        if (!Regex.IsMatch(sdt, @"^[0-9]{10}$"))
        {
            TempData["Error"] = "Số điện thoại phải đúng 10 chữ số";
            return RedirectToAction("Checkout");
        }
        // CHECK ĐỊA CHỈ
        if (!Regex.IsMatch(diaChi, @"^(?=.*\d)(?=.*[A-Za-zÀ-ỹ]).{10,}$"))
        {
            TempData["Error"] = "Địa chỉ phải gồm số nhà, tên đường, quận/huyện và tỉnh/thành";
            return RedirectToAction("Checkout");
        }


        if (ctghIds == null || !ctghIds.Any())
        {
            TempData["Error"] = "Vui lòng chọn sản phẩm để thanh toán";
            return RedirectToAction("Index", "GioHang");
        }

        var chiTiet = _context.ChiTietGioHangs
            .Include(x => x.MaDienThoaiNavigation)
            .Where(x => ctghIds.Contains(x.MaCtgh))
            .ToList();

        if (!chiTiet.Any())
            return RedirectToAction("Index", "GioHang");


        // ✅ TẠO HÓA ĐƠN (ĐÃ LƯU THÔNG TIN NGƯỜI NHẬN)
        var hoaDon = new HoaDon
        {
            MaHoaDon = "HD" + DateTime.Now.Ticks.ToString().Substring(10),
            MaKhachHang = maKH,
            NgayLap = DateOnly.FromDateTime(DateTime.Now),
            TongTien = chiTiet.Sum(x => x.ThanhTien ?? 0),
            TrangThai = 0,
            PhuongThucThanhToan = phuongThuc,

            TenNguoiNhan = tenNguoiNhan,
            SoDienThoai = sdt,
            DiaChiGiaoHang = diaChi
        };

        _context.HoaDons.Add(hoaDon);
        _context.SaveChanges();

        // ❗ CHỈ XÓA CÁC SẢN PHẨM ĐÃ THANH TOÁN
        _context.ChiTietGioHangs.RemoveRange(chiTiet);
        _context.SaveChanges();

        return RedirectToAction("Success");
    }

    [HttpPost]
    public IActionResult CheckoutSelected(List<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
        {
            TempData["Error"] = "Vui lòng chọn sản phẩm để thanh toán";
            return RedirectToAction("Index", "GioHang");
        }

        string maKH = "KH003";

        var chiTiet = _context.ChiTietGioHangs
            .Include(x => x.MaDienThoaiNavigation)
            .Include(x => x.MaGioHangNavigation)
            .Where(x => selectedIds.Contains(x.MaCtgh) &&
                        x.MaGioHangNavigation.MaKhachHang == maKH)
            .ToList();

        if (!chiTiet.Any())
            return RedirectToAction("Index", "GioHang");

        return View("Checkout", chiTiet);
    }

    public IActionResult Success()
    {
        return View();
    }
}
