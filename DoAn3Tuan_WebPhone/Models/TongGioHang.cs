using Microsoft.AspNetCore.Mvc;

namespace DoAn3Tuan_WebPhone.Models
{
    public class TongGioHang
    {
        public int TongSoLuongSP { get; set; } // Tổng số lượng sản phẩm
        public decimal TongTien { get; set; } // Tổng tiền giỏ hàng
    }
}
