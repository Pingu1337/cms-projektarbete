using Microsoft.JSInterop;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace frontend.Services
{
    public interface ILocalStorageService
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
        Task SetCookie<T>(string name, string value, int minutes);
        Task<string> GetCookie<T>(string key);
        Task DeleteCookie<T>(string name);
            
    }

    public class LocalStorageService : ILocalStorageService
    {
        private IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime; 
        }

        public async Task<T> GetItem<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (json == null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }
        public async Task SetItem<T>(string key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }

        public async Task SetCookie<T>(string name, string value, int minutes)
        {   
            await _jsRuntime.InvokeAsync<string>("blazorExtensions.WriteCookie", name, value, minutes);
        }

        public async Task DeleteCookie<T>(string name)
        {
            await _jsRuntime.InvokeAsync<string>("blazorExtensions.DeleteCookie", name);
        }

        public async Task<string> GetCookie<T>(string cname) 
        {
            var cookie = await _jsRuntime.InvokeAsync<string>("blazorExtensions.GetCookie", cname);

            if (cookie.Length <= 0)
                return null;

            return cookie;
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
