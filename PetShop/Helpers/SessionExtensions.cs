using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace PetShop.Helpers
{
    // Extension methods for storing and retrieving objects in session
    public static class SessionExtensions
    {
        // Save any object to session as JSON
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }

        // Read an object of type T from session; returns default(T) if not found
        public static T? GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}
