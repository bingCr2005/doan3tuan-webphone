using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoAn3Tuan_WebPhone.Controllers.Admin
{
    //kiểm tra đăng nhập Admin
    public class AdminBaseController : Controller
    {
        // Chạy trước khi vào các trang nội dung
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Lấy quyền Admin từ Session
            var role = context.HttpContext.Session.GetString("Role");

            // Nếu không phải Admin thì chuyển về trang Login
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                context.Result = new RedirectToActionResult("Index", "AdminLogin", null);
            }

            base.OnActionExecuting(context);
        }
    }
}