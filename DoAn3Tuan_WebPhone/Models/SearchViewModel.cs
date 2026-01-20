using Microsoft.AspNetCore.Mvc;

namespace DoAn3Tuan_WebPhone.Models
{
    public class SearchViewModel
    {
        // ===== FILTER =====
        public string? Keyword { get; set; }
        public string? Hang { get; set; }
        public decimal? GiaMin { get; set; }
        public decimal? GiaMax { get; set; }

        // ===== RESULT =====
        public List<DienThoai> DienThoais { get; set; } = new();

        // ===== PAGINATION =====
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 9;
    }
}
