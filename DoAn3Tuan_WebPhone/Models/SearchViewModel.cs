using Microsoft.AspNetCore.Mvc;

namespace DoAn3Tuan_WebPhone.Models
{
    public class SearchViewModel
    {
        // Kết quả trả về
        public List<DienThoai> Products { get; set; }
        public List<HangDienThoai> Brands { get; set; }

        // Tiêu chí tìm kiếm (Criteria)
        public string Keyword { get; set; }
        public string BrandId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Dữ liệu phân trang
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
