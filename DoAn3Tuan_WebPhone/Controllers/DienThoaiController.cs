using DoAn3Tuan_WebPhone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn3Tuan_WebPhone.Controllers
{
    public class DienThoaiController : Controller
    {
        private readonly DBBanDienThoaiContext _context;

        public DienThoaiController(DBBanDienThoaiContext context)
        {
            _context = context;
        }

        public IActionResult Search(
           string? keyword,
           string? hang,        // NHẬN MÃ HÃNG: HANG001, HANG002
           decimal? giaMin,
           decimal? giaMax,
           int page = 1)
        {
            int pageSize = 8;

            var query = _context.DienThoais
                .Where(x => x.TrangThai == 1 || x.TrangThai == null)
                .AsQueryable();

            // 🔍 SEARCH KEYWORD (GIỐNG TGDD)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();

                query = query.Where(x =>
                    x.TenDienThoai.ToLower().Contains(keyword) ||
                    (x.MoTa != null && x.MoTa.ToLower().Contains(keyword)) ||
                    (x.DungLuong != null && x.DungLuong.ToLower().Contains(keyword)) ||
                    (x.Ram != null && x.Ram.ToLower().Contains(keyword))
                );
            }

            // 🏷️ LỌC HÃNG — SO SÁNH TRỰC TIẾP MÃ HÃNG (CHUẨN DB)
            if (!string.IsNullOrWhiteSpace(hang))
            {
                hang = hang.Trim().ToLower();

                query = query.Where(x =>
                    x.HangDienThoaiNavigation != null &&
                    x.HangDienThoaiNavigation.TenHangDienThoai.ToLower() == hang
                );

            }

            // 💰 LỌC GIÁ
            if (giaMin.HasValue)
            {
                query = query.Where(x => x.DonGia >= giaMin.Value);
            }

            if (giaMax.HasValue)
            {
                query = query.Where(x => x.DonGia <= giaMax.Value);
            }

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var phones = query
                .OrderByDescending(x => x.LuotXem)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new SearchViewModel
            {
                Keyword = keyword,
                Hang = hang,
                GiaMin = giaMin,
                GiaMax = giaMax,
                CurrentPage = page,
                TotalPages = totalPages,
                DienThoais = phones
            };

            return View(vm);
        }


    }
}
