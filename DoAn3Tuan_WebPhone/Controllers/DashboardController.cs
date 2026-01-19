using DoAn3Tuan_WebPhone.Models;
using DoAn3Tuan_WebPhone.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller
{
    private readonly DBBanDienThoaiContext _context;

    public DashboardController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var model = new DashboardViewModel
        {
            TongDienThoai = _context.DienThoais.Count(),
            TongDonHang = _context.HoaDons.Count(),
            TongKhachHang = _context.TaiKhoanKhachHangs.Count(),
            TongLienHe = _context.LienHes.Count()
        };

        return View(model);
    }
}
