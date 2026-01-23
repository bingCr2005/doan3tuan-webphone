using Microsoft.AspNetCore.SignalR;
using DoAn3Tuan_WebPhone.Models;

namespace DoAn3Tuan_WebPhone.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DBBanDienThoaiContext _context;
        public ChatHub(DBBanDienThoaiContext context) { _context = context; }

        public async Task SendMessage(string user, string email, string message)
        {
            // 1. Lưu vào bảng TinNhanChat để không bị mất khi F5
            var chat = new TinNhanChat {
                HoTen = user,
                Email = email,
                NoiDung = message,
                NgayGui = DateTime.Now,
                TrangThai = 0
            };
            _context.TinNhanChats.Add(chat);
            await _context.SaveChangesAsync();

            // 2. Bắn tin nhắn đến TẤT CẢ mọi người đang mở web (Admin sẽ thấy ngay)
            await Clients.All.SendAsync("ReceiveMessage", user, message, chat.NgayGui?.ToString("HH:mm"));
        }
    }
}