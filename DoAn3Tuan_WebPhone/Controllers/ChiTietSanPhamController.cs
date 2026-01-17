using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;
using System.Linq;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class ChiTietSanPhamController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public ChiTietSanPhamController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        // GET: /ChiTietSanPham/Index/DT001
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            // 1️⃣ Lấy thông tin điện thoại
            var dienThoai = _context.DienThoais
                .FirstOrDefault(d => d.MaDienThoai == id);

            if (dienThoai == null)
                return NotFound();

            // ❗ Nếu có trạng thái ngừng bán (khuyên nên có)
            // if (dienThoai.TrangThai == 0)
            //     return RedirectToAction("HetHang");

            // 2️⃣ Lấy danh sách hình ảnh
            var hinhAnhs = _context.HinhAnhs
                .Where(h => h.MaDienThoai == id)
                .ToList();

            // 3️⃣ Lấy danh sách bình luận (đã duyệt)
            var binhLuans = _context.BinhLuans
                .Include(b => b.MaKhachHangNavigation)
                .Where(b => b.MaDienThoai == id && b.TrangThai == 1)
                .OrderByDescending(b => b.NgayBinhLuan)
                .ToList();

            // 4️⃣ Tính điểm trung bình sao
            double diemTB = binhLuans.Any()
            ? binhLuans.Average(b => (double?)b.SoSao) ?? 0
            : 0;


            // 5️⃣ Tăng lượt xem
            dienThoai.LuotXem += 1;
            _context.SaveChanges();

            // 6️⃣ Gán ViewModel
            var viewModel = new ChiTietSanPham
            {
                DienThoai = dienThoai,
                HinhAnhs = hinhAnhs,
                BinhLuans = binhLuans,
                DiemTrungBinh = diemTB,
                TongSoBinhLuan = binhLuans.Count,
                SoLuongYeuThich = dienThoai.SoLuongYeuThich,
                LuotXem = dienThoai.LuotXem
            };

            return View(viewModel);
        }
    }
}
