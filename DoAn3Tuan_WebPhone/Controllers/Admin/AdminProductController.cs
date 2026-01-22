using DoAn3Tuan_WebPhone.Models;
using DoAn3Tuan_WebPhone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AdminProductController : Controller
{
    private readonly DBBanDienThoaiContext _context;
    private readonly int _pageSize = 10;

    public AdminProductController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    //Tìm kiếm & Phân trang: Đọc dữ liệu từ SQL
    public async Task<IActionResult> Index(string searchString, int page = 1)
    {
        var hangList = _context.HangDienThoais.Select(h => new SelectListItem
        { Value = h.MaHangDienThoai, Text = h.TenHangDienThoai }).ToList();
        var nccList = _context.NhaCungCaps.Select(n => new SelectListItem
        { Value = n.MaNhaCungCap, Text = n.TenNhaCungCap }).ToList();

        if (vm != null)
        {
            vm.HangList = hangList;
            vm.NccList = nccList;
        }
    }

    // 2. Bảo mật: Kiểm tra quyền Admin cho toàn bộ Controller
    private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

    public async Task<IActionResult> Index(string searchString, int page = 1)
    {

        // Truyền dữ liệu phân trang ra View
        ViewBag.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)_pageSize);
          .Include(d => d.HangDienThoaiNavigation)
          .Include(d => d.MaNhaCungCapNavigation)
          .AsNoTracking()
          .AsQueryable();
        // Kiểm tra quyền Admin từ Session
    // GET Hiển thị Form Thêm mới
    public IActionResult Create()
            return RedirectToAction("Index", "AdminLogin");
        var vm = new AdminProductViewModel
        {
            // Chuyển dữ liệu từ DB sang danh sách hiển thị Dropdown
            HangList = _context.HangDienThoais.Select(h => new SelectListItem
            {
                Value = h.MaHangDienThoai,
                Text = h.TenHangDienThoai
            }),
            NccList = _context.NhaCungCaps.Select(n => new SelectListItem
            {
                Value = n.MaNhaCungCap,
                Text = n.TenNhaCungCap
            })
        };
            query = query.Where(s => s.TenDienThoai.Contains(searchString) || s.MaDienThoai.Contains(searchString));
        }

    //Thêm sản phẩm: Ghi mới vào SQL
    [HttpPost]
        ViewBag.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)_pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.Search = searchString;
        // Lưu Product trong ViewModel xuống Database
        if (ModelState.IsValid)
        return View("~/Views/Admin/AdminProduct/Index.cshtml", data);
    }

    public IActionResult Create()
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");

    //Hiển thị Form Sửa
    public async Task<IActionResult> Edit(string id)
        return View("~/Views/Admin/AdminProduct/CreateProduct.cshtml", vm);
    }

    [HttpPost]
        var vm = new AdminProductViewModel
        {
            Product = sp, // Đổ dữ liệu sp cần chỉnh sửa vào Form
            HangList = _context.HangDienThoais.Select(h => new SelectListItem
            {
                Value = h.MaHangDienThoai,
                Text = h.TenHangDienThoai,
                Selected = h.MaHangDienThoai == sp.HangDienThoai // Tự chọn đúng Hãng cũ
            }),
            NccList = _context.NhaCungCaps.Select(n => new SelectListItem
            {
                Value = n.MaNhaCungCap,
                Text = n.TenNhaCungCap,
                Selected = n.MaNhaCungCap == sp.MaNhaCungCap
            })
        };
    {
        if (ModelState.IsValid)
        {
    //Cập nhật sản phẩm
    [HttpPost]
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View("~/Views/Admin/AdminProduct/CreateProduct.cshtml", vm);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var sp = await _context.DienThoais.FindAsync(id);
        if (sp == null) return NotFound();

        var vm = new AdminProductViewModel { Product = sp };
    // Xóa sản phẩm: Xóa bản ghi khỏi SQL
    [HttpPost]
        return View("~/Views/Admin/AdminProduct/EditProduct.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
            _context.DienThoais.Remove(sp); // Lệnh DELETE trong SQL
            await _context.SaveChangesAsync();
        if (ModelState.IsValid)
        {
            _context.DienThoais.Update(vm.Product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View("~/Views/Admin/AdminProduct/EditProduct.cshtml", vm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var sp = await _context.DienThoais.FindAsync(id);
        if (sp != null)
        {
            _context.DienThoais.Remove(sp);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}