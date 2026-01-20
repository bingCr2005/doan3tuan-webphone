using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn3Tuan_WebPhone.Models;
using DoAn3Tuan_WebPhone.ViewModels;
using DoAn3Tuan_WebPhone.Helpers;// thêm để sử dụng session

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public HomeController(DBBanDienThoaiContext context)
        {
            _context = context;
        }
        // gioi thieu ve congty 
        public IActionResult About()
        {
            return View(); //Views/Home/About.cshtml
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;
            var viewModel = new HomePageViewModel();

            var baseQuery = _context.DienThoais.Where(p => p.TrangThai == 1);

            // lay du lieu cho slideShow
            var topViews = await baseQuery.OrderByDescending(p => p.LuotXem).Take(5).ToListAsync();

            viewModel.SanPhamNoiBat = new List<DienThoai>();
            //top view 17prm
            viewModel.SanPhamNoiBat.Add(topViews.First());
            viewModel.SanPhamNoiBat.Add(await baseQuery.OrderBy(p => p.DonGia).FirstOrDefaultAsync());
            viewModel.SanPhamNoiBat.Add(await baseQuery.FirstOrDefaultAsync(p => p.TenDienThoai.Contains("iPhone 17")));
           
            viewModel.SanPhamNoiBat.RemoveAll(item => item == null);

            // phân trang
            int totalItems = await baseQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            page = Math.Clamp(page, 1, totalPages > 0 ? totalPages : 1);


            //sap xep luot xem giam dan
            viewModel.SanPhamMoi = await baseQuery
                .Include(p => p.HangDienThoaiNavigation)
                .OrderByDescending(p => p.LuotXem) // view nhiều nhất thì lên trước
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            viewModel.DanhSachHang = await _context.HangDienThoais.ToListAsync();

            // Lấy 3 điện thoại có mã ID lớn nhất
            viewModel.SanPhamMoiVe = await _context.DienThoais
                .Where(p => p.TrangThai == 1)
                .OrderByDescending(p => p.MaDienThoai)
                .Take(3)
                .ToListAsync();

            // Bán Chạy laysa 3 sản phẩm có tổng số lượng bán nhiều nhất
            viewModel.SanPhamBanChay = await _context.DienThoais
                .Include(p => p.HangDienThoaiNavigation) // Để lấy tên hãng (thông tin thứ 4)
                .OrderByDescending(p => p.ChiTietHoaDons.Sum(ct => ct.SoLuong))
                .Take(3)
                            .ToListAsync();
            // Lấy chuỗi ID từ Session, nếu trống thì trả về chuỗi rỗng
            string currentViews = HttpContext.Session.GetString("RecentViews") ?? "";

            //Tách chuỗi thành List và xóa khoảng trắng (Trim) cho từng ID
            var dsId = currentViews.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(id => id.Trim())
                                    .ToList();

            viewModel.SanPhamDaXem = new List<DienThoai>();

            if (dsId.Any())
            {
                //Lấy sản phẩm từ Database
                var listSP = await _context.DienThoais
                    .Include(p => p.BinhLuans)
                    .Where(p => dsId.Contains(p.MaDienThoai)) // Database tự so sánh khớp mã
                    .ToListAsync();

                // Sắp xếp lại danh sách theo đúng thứ tự trong dsId và gán vào ViewModel
                viewModel.SanPhamDaXem = dsId
                    .Select(id => listSP.FirstOrDefault(sp => sp.MaDienThoai.Trim() == id))
                    .Where(sp => sp != null) // Bỏ qua nếu không tìm thấy máy (null)
                    .Cast<DienThoai>()     // Ép kiểu về đúng đối tượng điện thoại
                    .ToList();
            }
            return View(viewModel);
        }

        // phương thức xử lý đăng ký nhận bản tin (Newsletter)
        [HttpPost]
        public async Task<IActionResult> SubscribeNewsletter(string email)
        {
            try
            {
                // 1. Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(email))
                {
                    return Json(new { success = false, message = "Email không được để trống." });
                }

                // 2. Tạo đối tượng liên hệ mới để lưu vào Database(line he)
                var contact = new LienHe
                {
                    HoTen = "Khách Newsletter", // Tên mặc định cho người đăng ký qua form newsletter
                    Email = email,
                    SoDienThoai = "N/A",
                    NoiDung = "Đăng ký nhận bản tin khuyến mãi 500.000đ từ trang chủ.",
                    NgayGui = DateTime.Now,
                    TrangThai = 0 // 0: Mới, 1: Đã xử lý
                };

                // 3. Lưu vào cơ sở dữ liệu
                _context.LienHes.Add(contact);
                await _context.SaveChangesAsync();

                // 4. Trả về kết quả thành công cho AJAX
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Trả về lỗi nếu có sự cố
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}