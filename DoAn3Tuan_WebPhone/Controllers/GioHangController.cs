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
    public IActionResult Update(int id, int qty, int page = 1)
    {
        if (qty < 1) qty = 1;

        var ct = _context.ChiTietGioHangs
            .Include(x => x.MaDienThoaiNavigation)
            .FirstOrDefault(x => x.MaCtgh == id);

        if (ct != null)
        {
            ct.SoLuong = qty;
            ct.ThanhTien = qty * ct.MaDienThoaiNavigation!.DonGia;
            _context.SaveChanges();
        }

        return RedirectToAction("Index", new { page });
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
    public IActionResult AddToCart(string maDT)
    {
        string maKH = "KH003";

        var gioHang = _context.GioHangs
            .Include(g => g.ChiTietGioHangs)
            .FirstOrDefault(g => g.MaKhachHang == maKH);

        if (gioHang == null)
        {
            gioHang = new GioHang
            {
                MaGioHang = "GH" + DateTime.Now.Ticks.ToString().Substring(10),
                MaKhachHang = maKH
            };
            _context.GioHangs.Add(gioHang);
            _context.SaveChanges();
        }

        var ct = _context.ChiTietGioHangs
            .Include(x => x.MaDienThoaiNavigation)
            .FirstOrDefault(x => x.MaGioHang == gioHang.MaGioHang &&
                                 x.MaDienThoai == maDT);

        if (ct == null)
        {
            var dt = _context.DienThoais.Find(maDT);
            if (dt == null) return NotFound();

            ct = new ChiTietGioHang
            {
                MaGioHang = gioHang.MaGioHang,
                MaDienThoai = maDT,
                SoLuong = 1,
                ThanhTien = dt.DonGia
            };
            _context.ChiTietGioHangs.Add(ct);
        }
        else
        {
            ct.SoLuong++;
            ct.ThanhTien = ct.SoLuong * ct.MaDienThoaiNavigation!.DonGia;
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
