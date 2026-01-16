using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public ThongKeController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // View chính
        public IActionResult Index()
        {
            var hoaDons = _context.HoaDons
                .Where(h => h.NgayLap != null)
                .ToList();

            ViewBag.TongDoanhThu = hoaDons.Sum(x => x.TongTien);
            ViewBag.TongLuotMua = hoaDons.Count;

            // Mặc định thống kê theo tháng
            ViewBag.DoanhThuThang = hoaDons
                .GroupBy(h => h.NgayLap!.Value.Month)
                .Select(g => new
                {
                    thang = g.Key,
                    tongTien = g.Sum(x => x.TongTien),
                    luotMua = g.Count()
                })
                .OrderBy(x => x.thang)
                .ToList();

            return View();
        }

        // API lấy dữ liệu cho Chart.js
        [HttpGet]
        public IActionResult GetThongKe(string type = "month")
        {
            var hoaDons = _context.HoaDons
                .Where(h => h.NgayLap != null)
                .ToList();

            object data;

            switch (type)
            {
                // ===== THEO NGÀY =====
                case "day":
                    data = hoaDons
                        .GroupBy(h => h.NgayLap!.Value)
                        .Select(g => new
                        {
                            label = g.Key.ToString("dd/MM/yyyy"),
                            tongTien = g.Sum(x => x.TongTien),
                            luotMua = g.Count()
                        })
                        .OrderBy(x =>
                            DateTime.ParseExact(x.label, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                        .ToList();
                    break;

                // ===== THEO TUẦN =====
                case "week":
                    data = hoaDons
                        .GroupBy(h =>
                            ISOWeek.GetWeekOfYear(
                                h.NgayLap!.Value.ToDateTime(TimeOnly.MinValue)))
                        .Select(g => new
                        {
                            label = "Tuần " + g.Key,
                            tongTien = g.Sum(x => x.TongTien),
                            luotMua = g.Count()
                        })
                        .OrderBy(x => x.label)
                        .ToList();
                    break;

                // ===== THEO THÁNG =====
                default:
                    data = hoaDons
                        .GroupBy(h => h.NgayLap!.Value.Month)
                        .Select(g => new
                        {
                            label = "Tháng " + g.Key,
                            tongTien = g.Sum(x => x.TongTien),
                            luotMua = g.Count()
                        })
                        .OrderBy(x => x.label)
                        .ToList();
                    break;
            }

            return Json(new
            {
                tongDoanhThu = hoaDons.Sum(x => x.TongTien),
                tongLuotMua = hoaDons.Count,
                data
            });
        }
    }
}
