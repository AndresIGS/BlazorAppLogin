using Blazored.SessionStorage;
using System.Text.Json;
using System.Text;
using BlazorAppLogin.Shared;

namespace BlazorAppLogin.Client.Models
{
    public static class SesionStorage
    {
        public static async Task GuardarStorage<T>(this ISessionStorageService sessionStorageService, string key, T item) where T : class
        {
            var itemJson = JsonSerializer.Serialize(item);
            await sessionStorageService.SetItemAsStringAsync(key, itemJson);
        }

        public static async Task<T?> ObtenerStorage<T>(this ISessionStorageService sessionStorageService, string key) where T : class
        {
            var itemJson = await sessionStorageService.GetItemAsStringAsync(key);

            if (itemJson != null)
            {
                var deserializedItem = JsonSerializer.Deserialize<T>(itemJson);
                return deserializedItem;
            }
            else
            {
                return null;
            }
        }
    }
}