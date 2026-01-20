using Newtonsoft.Json;

namespace DoAn3Tuan_WebPhone.Helpers
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            // Dùng Newtonsoft để "ép" dữ liệu vào chuỗi JSON chuẩn
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            // Giải mã chuỗi JSON ngược lại thành Object
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}