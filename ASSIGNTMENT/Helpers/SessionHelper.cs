using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ASSIGNTMENT.Helpers
{
    public static class SessionHelper
    {
        // 👉 Lưu object vào session
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // 👉 Lấy object từ session
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            if (value == null)
                return default;

             return JsonSerializer.Deserialize<T>(value);
        }
    }
}