using System.Runtime.CompilerServices;
using System.Text.Json;

namespace demoapp.Services
{
    public static class SessionExtension
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var objet = session.GetString(key);
            return objet == null ? default : JsonSerializer.Deserialize<T>(objet);
        }

    }
}
