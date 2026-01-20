using Microsoft.AspNetCore.Mvc;

namespace DoAn3Tuan_WebPhone.ViewModels
{
    public class RegisterViewModel
    {
        public string HoVaTen { get; set; }
        public string TenDangNhap { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
