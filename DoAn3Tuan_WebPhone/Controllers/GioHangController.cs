using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;
using System;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class GioHangController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public GioHangController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // 24 + 27: Hiển thị giỏ hàng + phân trang
        public IActionResult Index(int page = 1)
        {
            //xoá được á mà tui làm biến thêm lại csdl :)))
           

            string maKH = "KH002"; /*= HttpContext.Session.GetString("MaKH");

            if (maKH == null)
            {
                return RedirectToAction("Login", "Account");
            }
               */

            int pageSize = 5;

            var query = _context.GioHangs
                .Include(g => g.MaDienThoaiNavigation)
                .Where(g => g.MaKhachHang == maKH);

            ViewBag.TotalPage = Math.Ceiling(query.Count() / (double)pageSize);
            ViewBag.CurrentPage = page;

            var data = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(data);
        }

        // 25: Cập nhật số lượng
        [HttpPost]
        public IActionResult Update(string id, int qty)
        {
            if (qty < 1) qty = 1;

            var gh = _context.GioHangs
                .Include(x => x.MaDienThoaiNavigation)
                .FirstOrDefault(x => x.MaGioHang == id);

            if (gh != null && gh.MaDienThoaiNavigation != null)
            {
                gh.SoLuongHang = qty;
                gh.ThanhTien = qty * gh.MaDienThoaiNavigation.DonGia;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Xóa 1 sản phẩm trong giỏ
        public IActionResult Delete(string id)
        {
            var gh = _context.GioHangs.Find(id);
            if (gh != null)
            {
                _context.GioHangs.Remove(gh);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // 26: Xóa hết giỏ hàng
        public IActionResult Clear()
        {
            string maKH = "KH01";

            var list = _context.GioHangs
                .Where(x => x.MaKhachHang == maKH);

            _context.GioHangs.RemoveRange(list);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Thêm sản phẩm vào giỏ
        public IActionResult AddToCart(string maDT)
        {
            string maKH = "KH01";

            var gh = _context.GioHangs
                .Include(x => x.MaDienThoaiNavigation)
                .FirstOrDefault(x => x.MaKhachHang == maKH &&
                                     x.MaDienThoai == maDT);

            if (gh == null)
            {
                var dt = _context.DienThoais.Find(maDT);
                if (dt == null)
                    return NotFound();

                gh = new GioHang
                {
                    MaGioHang = Guid.NewGuid().ToString("N").Substring(0, 10),
                    MaKhachHang = maKH,
                    MaDienThoai = maDT,
                    SoLuongHang = 1,
                    ThanhTien = dt.DonGia
                };

                _context.GioHangs.Add(gh);
            }
            else if (gh.MaDienThoaiNavigation != null)
            {
                gh.SoLuongHang++;
                gh.ThanhTien =
                    gh.SoLuongHang * gh.MaDienThoaiNavigation.DonGia;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
