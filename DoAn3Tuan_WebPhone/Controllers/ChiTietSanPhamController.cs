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

            // 1️ Lấy thông tin điện thoại
            var dienThoai = _context.DienThoais
                .AsNoTracking()
                .FirstOrDefault(d => d.MaDienThoai == id);

            if (dienThoai == null)
                return NotFound();

            // 2️ Lấy danh sách hình ảnh
            var hinhAnhs = _context.HinhAnhs
                .Where(h => h.MaDienThoai == id)
                .AsNoTracking()
                .ToList();

            // 3️ Lấy danh sách bình luận (đã duyệt)
            var binhLuans = _context.BinhLuans
                .Include(b => b.MaKhachHangNavigation)
                    .ThenInclude(kh => kh.MaKhachHangNavigation)
                .Where(b => b.MaDienThoai == id && b.TrangThai == 1)
                .OrderByDescending(b => b.NgayBinhLuan)
                .AsNoTracking()
                .ToList();

            // 4️ Tính điểm trung bình sao
            double diemTB = binhLuans.Any()
                ? binhLuans.Average(b => (double)b.SoSao)
                : 0;

            // 5️ Tăng lượt xem
            _context.DienThoais
                .Where(d => d.MaDienThoai == id)
                .ExecuteUpdate(d =>
                    d.SetProperty(x => x.LuotXem, x => x.LuotXem + 1)
                );

            // 6️ LẤY SẢN PHẨM LIÊN QUAN THEO HÃNG 📱
            var sanPhamLienQuan = _context.DienThoais
                .Where(d =>
                    d.HangDienThoai == dienThoai.HangDienThoai &&          // 👈 CÙNG HÃNG
                    d.MaDienThoai != dienThoai.MaDienThoai)  // ❌ loại trừ chính nó
                .OrderByDescending(d => d.LuotXem)          // ưu tiên xem nhiều
                .Take(3)                                    // giới hạn số lượng
                .AsNoTracking()
                .ToList();

            // 7️ Gán ViewModel
            var viewModel = new ChiTietSanPham
            {
                DienThoai = dienThoai,
                HinhAnhs = hinhAnhs,
                BinhLuans = binhLuans,
                DiemTrungBinh = diemTB,
                TongSoBinhLuan = binhLuans.Count,
                SoLuongYeuThich = dienThoai.SoLuongYeuThich,
                LuotXem = dienThoai.LuotXem,
                SanPhamLienQuan = sanPhamLienQuan // ✅ QUAN TRỌNG
            };

            return View(viewModel);
        }
    }
}
