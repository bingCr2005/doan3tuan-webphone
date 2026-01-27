using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;
using System.Linq;
using DoAn3Tuan_WebPhone.Helpers;// su dụng GetJson/SetJson

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

            // 1. Loại bỏ khoảng trắng thừa của ID sản phẩm (Xử lý vấn đề kiểu CHAR(10) trong SQL)
            string cleanId = id.Trim();

            // 2. Lấy danh sách ID cũ từ túi áo (Session). Nếu chưa có thì tạo chuỗi rỗng ""
            string currentViews = HttpContext.Session.GetString("RecentViews") ?? "";

            // 3. Chuyển chuỗi ID thành một danh sách (List) để dễ xử lý (thêm, xóa, sắp xếp)
            List<string> dsId = currentViews.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                             .Select(x => x.Trim()).ToList();

            // 4. Nếu sản phẩm này đã có trong danh sách thì xóa đi 
            // Mục đích: Để khi Insert lại, nó sẽ nhảy lên vị trí đầu tiên (vừa xem xong)
            if (dsId.Contains(cleanId)) dsId.Remove(cleanId);

            // 5. Thêm ID sản phẩm hiện tại vào vị trí đầu tiên (vị trí số 0)
            dsId.Insert(0, cleanId);

            // 6. Chỉ giữ lại tối đa 3 sản phẩm xem gần nhất
            if (dsId.Count > 3) dsId = dsId.Take(3).ToList();

            // 7. Gộp danh sách ID lại thành một chuỗi duy nhất (ngăn cách bởi dấu phẩy) và cất vào túi áo
            HttpContext.Session.SetString("RecentViews", string.Join(",", dsId));

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

            // 6️ LẤY SẢN PHẨM LIÊN QUAN THEO HÃNG
            var sanPhamLienQuan = _context.DienThoais
                .Where(d =>
                    d.HangDienThoai == dienThoai.HangDienThoai &&  
                    d.MaDienThoai != dienThoai.MaDienThoai) 
                .OrderByDescending(d => d.LuotXem)          
                .Take(3)                                
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
                SanPhamLienQuan = sanPhamLienQuan //QUAN TRỌNG
            };

            return View(viewModel);
        }
    }
}
