using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

public class ThanhToanController : Controller
{
    private readonly DBBanDienThoaiContext _context;

    public ThanhToanController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    //  CHECKOUT 
    public IActionResult Checkout()
    {
        string maKH = "KH003";

        var gioHang = _context.GioHangs
            .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.MaDienThoaiNavigation)
            .FirstOrDefault(g => g.MaKhachHang == maKH);

        if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            return RedirectToAction("Index", "GioHang");

        return View(gioHang.ChiTietGioHangs.ToList());
    }

    [HttpPost]
    public IActionResult Confirm(
        List<int> ctghIds,
        string tenNguoiNhan,
        string sdt,
        string diaChi,
        string phuongThuc,
        string maGiamGia)
    {
        string maKH = "KH003";
        if (ctghIds == null || !ctghIds.Any())
            return Content("KHÔNG NHẬN ĐƯỢC ctghIds");


        if (ctghIds == null || !ctghIds.Any())
            return RedirectToAction("Index", "GioHang");

        if (string.IsNullOrWhiteSpace(tenNguoiNhan) ||
            string.IsNullOrWhiteSpace(sdt) ||
            string.IsNullOrWhiteSpace(diaChi))
            return RedirectToAction("Checkout");

        if (!Regex.IsMatch(sdt, @"^[0-9]{10}$"))
            return RedirectToAction("Checkout");

        IActionResult? result = null;

        var strategy = _context.Database.CreateExecutionStrategy();

        strategy.Execute(() =>
        {
            using var tran = _context.Database.BeginTransaction();

            try
            {
                // ===== LẤY GIỎ HÀNG =====
                var gioHang = _context.GioHangs
                    .Include(g => g.ChiTietGioHangs)
                        .ThenInclude(ct => ct.MaDienThoaiNavigation)
                    .FirstOrDefault(g => g.MaKhachHang == maKH);

                if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
                {
                    result = RedirectToAction("Index", "GioHang");
                    return;
                }

                var chiTiet = gioHang.ChiTietGioHangs
                    .Where(x => ctghIds.Contains(x.MaCtgh))
                    .ToList();

                if (!chiTiet.Any())
                {
                    result = RedirectToAction("Index", "GioHang");
                    return;
                }

                // ===== KHUYẾN MÃI =====
                KhuyenMai? km = null;
                decimal tienGiam = 0;

                if (!string.IsNullOrWhiteSpace(maGiamGia))
                {
                    km = _context.KhuyenMais.FirstOrDefault(x =>
                        x.MaKhuyenMai == maGiamGia &&
                        x.TrangThai == 1 &&
                        x.NgayBatDau <= DateOnly.FromDateTime(DateTime.Now) &&
                        x.NgayHetHan >= DateOnly.FromDateTime(DateTime.Now));

                    if (km != null)
                        tienGiam = km.MucGiamGia ?? 0;
                }

                decimal tongGoc = chiTiet.Sum(x => x.ThanhTien ?? 0);
                decimal tongSauGiam = Math.Max(0, tongGoc - tienGiam);

                // ===== TẠO HÓA ĐƠN =====
                int lastHD = _context.HoaDons
                    .AsEnumerable()
                    .Select(x => int.Parse(x.MaHoaDon.Replace("HD", "")))
                    .DefaultIfEmpty(0)
                    .Max();

                string maHoaDon = "HD" + (lastHD + 1).ToString("D3");

                var hoaDon = new HoaDon
                {
                    MaHoaDon = maHoaDon,
                    MaKhachHang = maKH,
                    MaAdmin = "AD001",
                    NgayLap = DateOnly.FromDateTime(DateTime.Now),
                    TongTien = tongSauGiam,
                    TrangThai = 0,
                    PhuongThucThanhToan = phuongThuc,
                    TenNguoiNhan = tenNguoiNhan,
                    SoDienThoai = sdt,
                    DiaChiGiaoHang = diaChi,
                    MaKhuyenMai = km?.MaKhuyenMai
                };

                _context.HoaDons.Add(hoaDon);
                _context.SaveChanges();

                // ===== CHI TIẾT HÓA ĐƠN =====
                foreach (var item in chiTiet)
                {
                    _context.ChiTietHoaDons.Add(new ChiTietHoaDon
                    {
                        MaHoaDon = maHoaDon,
                        MaDienThoai = item.MaDienThoai,
                        SoLuong = item.SoLuong,
                        DonGia = item.MaDienThoaiNavigation!.DonGia,
                        ThanhTien = item.ThanhTien,
                        TrangThai = 0,
                        MaKhuyenMai = km?.MaKhuyenMai
                    });
                }

                if (km != null)
                    km.SoLuongMaKhuyenMaiDaDung++;

                _context.ChiTietGioHangs.RemoveRange(chiTiet);

                _context.SaveChanges();
                tran.Commit();

                result = RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                tran.Rollback();
                result = Content("LỖI THANH TOÁN: " + ex.Message);
            }
        });

        return result ?? RedirectToAction("Checkout");
    }



    public IActionResult Success()
    {
        return View();
    }


}
