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
        var query = _context.DienThoais
            .Include(d => d.HangDienThoaiNavigation)
            .Include(d => d.MaNhaCungCapNavigation)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(s => s.TenDienThoai.Contains(searchString) || s.MaDienThoai.Contains(searchString));
        }

        var data = await query.Skip((page - 1) * _pageSize).Take(_pageSize).ToListAsync();

        // Truyền dữ liệu phân trang ra View
        ViewBag.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)_pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.Search = searchString;

        return View("~/Views/Admin/AdminProduct/Index.cshtml", data);
    }
    // GET Hiển thị Form Thêm mới
    public IActionResult Create()
    {
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
        return View("~/Views/Admin/AdminProduct/CreateProduct.cshtml", vm);
    }

    //Thêm sản phẩm: Ghi mới vào SQL
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminProductViewModel vm)
    {
        // Lưu Product trong ViewModel xuống Database
        if (ModelState.IsValid)
        {
            _context.DienThoais.Add(vm.Product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View("~/Views/Admin/AdminProduct/CreateProduct.cshtml", vm);
    }
    //Hiển thị Form Sửa
    public async Task<IActionResult> Edit(string id)
    {
        var sp = await _context.DienThoais.FindAsync(id);
        if (sp == null) return NotFound();

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
        return View("~/Views/Admin/AdminProduct/EditProduct.cshtml", vm);
    }

    //Cập nhật sản phẩm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AdminProductViewModel vm)
    {
        if (ModelState.IsValid)
        {
            _context.DienThoais.Update(vm.Product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View("~/Views/Admin/AdminProduct/EditProduct.cshtml", vm);
    }

    // Xóa sản phẩm: Xóa bản ghi khỏi SQL
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var sp = await _context.DienThoais.FindAsync(id);
        if (sp != null)
        {
            _context.DienThoais.Remove(sp); // Lệnh DELETE trong SQL
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}