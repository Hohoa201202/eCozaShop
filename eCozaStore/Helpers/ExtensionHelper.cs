using Microsoft.AspNetCore.Http;
using System.Text.Json;


namespace eCozaStore.Helpers
{
    public static class ExtensionHelper
    {
        public static string ToVnd(int value)
        {
            return $"{value:#,##0.00} đ";
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
