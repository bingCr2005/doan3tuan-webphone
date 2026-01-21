using DoAn3Tuan_WebPhone.Models;
using DoAn3Tuan_WebPhone.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
[Area("Admin")]
public class AdminProductController : Controller
{
    private readonly DBBanDienThoaiContext _context;
    private readonly int _pageSize = 10;

    public AdminProductController(DBBanDienThoaiContext context)
    {
        _context = context;
    }

    // 1. Hàm bổ trợ: Tải lại danh sách Hãng và NCC để tránh lặp code
    private void PopulateDropdowns(AdminProductViewModel vm = null)
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
        if (!IsAdmin()) return RedirectToAction("Index", "Home"); //

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
        ViewBag.TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)_pageSize);
        ViewBag.CurrentPage = page;
        ViewBag.Search = searchString;

        return View("~/Views/Admin/AdminProduct/Index.cshtml", data);
    }

    public IActionResult Create()
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");

        var vm = new AdminProductViewModel();
        PopulateDropdowns(vm); // Tải danh sách cho Dropdown
        return View("~/Views/Admin/AdminProduct/CreateProduct.cshtml", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminProductViewModel vm)
    {
        if (ModelState.IsValid)
        {
            _context.DienThoais.Add(vm.Product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // LƯU Ý: Nếu lỗi, phải nạp lại danh sách Dropdown nếu không View sẽ crash
        PopulateDropdowns(vm);
        return View("~/Views/Admin/AdminProduct/CreateProduct.cshtml", vm);
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");

        var sp = await _context.DienThoais.FindAsync(id);
        if (sp == null) return NotFound();

        var vm = new AdminProductViewModel { Product = sp };
        PopulateDropdowns(vm);
        return View("~/Views/Admin/AdminProduct/EditProduct.cshtml", vm);
    }

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

        PopulateDropdowns(vm);
        return View("~/Views/Admin/AdminProduct/EditProduct.cshtml", vm);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (!IsAdmin()) return RedirectToAction("Index", "Home");

        var sp = await _context.DienThoais.FindAsync(id);
        if (sp != null)
        {
            _context.DienThoais.Remove(sp);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}