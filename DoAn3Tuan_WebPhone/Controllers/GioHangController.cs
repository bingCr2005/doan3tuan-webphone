using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;
using System;
using System.Linq;

public class GioHangController : Controller
{
    private readonly DbbanDienThoaiContext _context;

    public GioHangController(DbbanDienThoaiContext context)
    {
        _context = context;
    }

    // HIỂN THỊ GIỎ HÀNG
    public IActionResult Index()
    {
        string maKH = "KH003";

        var gioHang = _context.GioHangs
            .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.MaDienThoaiNavigation)
            .FirstOrDefault(g => g.MaKhachHang == maKH);

        if (gioHang == null)
            return View(new List<ChiTietGioHang>());

        return View(gioHang.ChiTietGioHangs.ToList());
    }

    // CẬP NHẬT SỐ LƯỢNG
    [HttpPost]
    public IActionResult Update(int id, int qty)
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

        return RedirectToAction("Index");
    }

    // XÓA 1 SẢN PHẨM
    public IActionResult Delete(int id)
    {
        var ct = _context.ChiTietGioHangs.Find(id);
        if (ct != null)
        {
            _context.ChiTietGioHangs.Remove(ct);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
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
