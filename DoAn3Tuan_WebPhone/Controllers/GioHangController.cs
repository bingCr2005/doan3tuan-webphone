using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;
using System;
using System.Linq;

public class GioHangController : Controller
{
    private readonly DBBanDienThoaiContext _context;

    public GioHangController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    // HIỂN THỊ GIỎ HÀNG
    public IActionResult Index(int page = 1)
    {
        string maKH = "KH003";
        int pageSize = 10;

        var gioHang = _context.GioHangs
            .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.MaDienThoaiNavigation)
            .FirstOrDefault(g => g.MaKhachHang == maKH);

        if (gioHang == null)
        {
            ViewBag.TotalPages = 0;
            ViewBag.CurrentPage = page;
            return View(new List<ChiTietGioHang>());
        }

        var query = gioHang.ChiTietGioHangs.AsQueryable();

        int totalItems = query.Count();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = query
            .OrderBy(x => x.MaCtgh)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.TotalPages = totalPages;
        ViewBag.CurrentPage = page;

        return View(items);
    }

    // CẬP NHẬT SỐ LƯỢNG
    [HttpPost]
    public IActionResult Update(int id, int qty)
    {
        if (qty < 1) qty = 1;

        var ct = _context.ChiTietGioHangs
            .Include(x => x.MaDienThoaiNavigation)
            .FirstOrDefault(x => x.MaCtgh == id);

        if (ct == null)
            return Json(new { success = false, message = "Không tìm thấy sản phẩm" });

        int tonKho = ct.MaDienThoaiNavigation!.SoLuongTon ?? 0;

        if (qty > tonKho)
        {
            ct.SoLuong = tonKho;
            ct.ThanhTien = tonKho * ct.MaDienThoaiNavigation.DonGia;
            _context.SaveChanges();

            return Json(new
            {
                success = false,
                message = $" Chỉ còn {tonKho} sản phẩm trong kho. Đã đặt số lượng tối đa."
            });
        }

        ct.SoLuong = qty;
        ct.ThanhTien = qty * ct.MaDienThoaiNavigation.DonGia;
        _context.SaveChanges();

        return Json(new { success = true });
    }

    // XÓA 1 SẢN PHẨM
    public IActionResult Delete(int id, int page = 1)
    {
        var ct = _context.ChiTietGioHangs.Find(id);
        if (ct != null)
        {
            _context.ChiTietGioHangs.Remove(ct);
            _context.SaveChanges();
        }
        return RedirectToAction("Index", new { page });
    }

    // XÓA HẾT GIỎ
    public IActionResult Clear()
    {
        string maKH = "KH003";

        var gioHang = _context.GioHangs
            .Include(g => g.ChiTietGioHangs)
            .FirstOrDefault(g => g.MaKhachHang == maKH);

        if (gioHang != null)
        {
            _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    // THÊM SẢN PHẨM
    [HttpPost]
    public IActionResult AddToCart(string maDT, int qty)
    {
        string maKH = "KH003";

        var gioHang = _context.GioHangs
            .Include(g => g.ChiTietGioHangs)
            .FirstOrDefault(g => g.MaKhachHang == maKH);

        if (gioHang == null)
        {
            gioHang = new GioHang { MaKhachHang = maKH };
            _context.GioHangs.Add(gioHang);
            _context.SaveChanges();
        }

        var sanPham = _context.DienThoais.FirstOrDefault(x => x.MaDienThoai == maDT);
        if (sanPham == null)
            return Json(new { success = false, message = "Sản phẩm không tồn tại" });

        if (sanPham.SoLuongTon < qty)
            return Json(new { success = false, message = "Không đủ số lượng tồn" });

        var ct = gioHang.ChiTietGioHangs
            .FirstOrDefault(x => x.MaDienThoai == maDT);

        if (ct != null)
        {
            ct.SoLuong += qty;
        }
        else
        {
            ct = new ChiTietGioHang
            {
                MaGioHang = gioHang.MaGioHang,
                MaDienThoai = maDT,
                SoLuong = qty
            };
            _context.ChiTietGioHangs.Add(ct);
        }

        // 🔥 TRỪ TỒN KHO THẬT
        sanPham.SoLuongTon -= qty;

        // cập nhật thành tiền
        ct.ThanhTien = ct.SoLuong * sanPham.DonGia;

        _context.SaveChanges();

        return Json(new
        {
            success = true,
            message = "Đã thêm vào giỏ hàng",
            newStock = sanPham.SoLuongTon // 🔥 trả tồn kho mới
        });
    }


}
