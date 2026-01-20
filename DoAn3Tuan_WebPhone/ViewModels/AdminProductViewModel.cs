using Microsoft.AspNetCore.Mvc.Rendering;
using DoAn3Tuan_WebPhone.Models;

namespace DoAn3Tuan_WebPhone.ViewModels
{
    public class AdminProductViewModel
    {
        // Chứa 1 đối tượng điện thoại để Thêm/Sửa
        public DienThoai Product { get; set; } = new DienThoai();

        // Danh sách Hãng và NCC để đổ vào Dropdown
        public IEnumerable<SelectListItem>? HangList { get; set; }
        public IEnumerable<SelectListItem>? NccList { get; set; }
    }
}