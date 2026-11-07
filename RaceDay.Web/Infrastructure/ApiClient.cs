using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaceDay.Web.Infrastructure;

public sealed class ApiClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _accessor;
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = true };

    public ApiClient(HttpClient http, IHttpContextAccessor accessor) { _http = http; _accessor = accessor; }

    public async Task<ApiResult<T>> GetAsync<T>(string path) => await SendAsync<T>(HttpMethod.Get, path, null);
    public async Task<ApiResult<T>> PostAsync<T>(string path, object? body = null) => await SendAsync<T>(HttpMethod.Post, path, body);
    public async Task<ApiResult<T>> PutAsync<T>(string path, object? body = null) => await SendAsync<T>(HttpMethod.Put, path, body);
    public async Task<ApiResult<T>> DeleteAsync<T>(string path) => await SendAsync<T>(HttpMethod.Delete, path, null);

    public async Task<ApiResult<T>> UploadAsync<T>(string path, IFormFile file)
    {
        using var form = new MultipartFormDataContent();
        await using var stream = file.OpenReadStream();
        using var content = new StreamContent(stream);
        content.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
        form.Add(content, "file", Path.GetFileName(file.FileName));
        using var request = new HttpRequestMessage(HttpMethod.Post, path) { Content = form };
        return await SendRequestAsync<T>(request);
    }

    private async Task<ApiResult<T>> SendAsync<T>(HttpMethod method, string path, object? body)
    {
        using var request = new HttpRequestMessage(method, path);
        if (body != null) request.Content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
        return await SendRequestAsync<T>(request);
    }

    private async Task<ApiResult<T>> SendRequestAsync<T>(HttpRequestMessage request)
    {
        var context = _accessor.HttpContext;
        if (context != null)
        {
            var cookie = context.Session.GetString("ApiSessionCookie");
            if (!string.IsNullOrWhiteSpace(cookie)) request.Headers.TryAddWithoutValidation("Cookie", cookie);
        }
        try
        {
            using var response = await _http.SendAsync(request);
            if (context != null && response.Headers.TryGetValues("Set-Cookie", out var setCookies))
            {
                var apiCookie = setCookies.Select(x => x.Split(';', 2)[0]).FirstOrDefault(x => x.StartsWith("RaceDaySession=", StringComparison.Ordinal));
                if (!string.IsNullOrWhiteSpace(apiCookie)) context.Session.SetString("ApiSessionCookie", apiCookie);
            }
            var raw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                var message = raw;
                try { using var doc = JsonDocument.Parse(raw); if (doc.RootElement.TryGetProperty("message", out var m)) message = m.GetString() ?? raw; } catch { }
                return new ApiResult<T>(false, default, message, (int)response.StatusCode);
            }
            if (typeof(T) == typeof(string)) return new ApiResult<T>(true, (T)(object)raw, "", (int)response.StatusCode);
            if (string.IsNullOrWhiteSpace(raw)) return new ApiResult<T>(true, default, "", (int)response.StatusCode);
            var data = JsonSerializer.Deserialize<T>(raw, JsonOptions);
            return new ApiResult<T>(true, data, "", (int)response.StatusCode);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return new ApiResult<T>(false, default, "Cannot connect to the RaceDay API. Start RaceDay.API and check ApiSettings:BaseUrl.", 0);
        }
    }
}

public sealed record ApiResult<T>(bool Success, T? Data, string Error, int StatusCode);
