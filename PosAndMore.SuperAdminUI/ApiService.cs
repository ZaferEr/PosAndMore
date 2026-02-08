using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // ValidationProblemDetails için
using Microsoft.Extensions.Options;
using PosAndMore.SuperAdmin.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PosAndMore.SuperAdminUI // namespace'i kendi projene göre değiştir
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RestoranApi");
        }

        /// <summary>
        /// Bearer token'ı header'a ekler (gelecekteki tüm isteklerde kullanılır)
        /// </summary>
        public void SetAuthorizationHeader(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                return;
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Ortak deserialize options (tekrar yazmamak için)
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // GET
        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint, CancellationToken ct = default)
        {
            try
            {
                var httpResponse = await _httpClient.GetAsync(endpoint, ct);
                var statusCode = (int)httpResponse.StatusCode;
                var contentString = await httpResponse.Content.ReadAsStringAsync(ct);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<T>>(JsonOptions, ct);

                    if (apiResponse == null)
                    {
                        return ApiResponse<T>.Failure(statusCode, $"Deserialize edilemedi. Ham içerik: {contentString}", null);
                    }

                    // StatusCode 0 ise HTTP status ile güncelle
                    if (apiResponse.StatusCode == 0 || apiResponse.StatusCode == default)
                    {
                        apiResponse = apiResponse with { StatusCode = statusCode };
                    }

                    return apiResponse;
                }

                // Hata durumu
                return await HandleErrorResponse<T>(statusCode, contentString);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                return ApiResponse<T>.Failure(503, "Sunucuya bağlanılamadı – internetini kontrol et ponçik 😏", null);
            }
            catch (TaskCanceledException)
            {
                return ApiResponse<T>.Failure(408, "İstek zaman aşımına uğradı, yavaş yavaş geliyorsun galiba 😉", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<T>.FromException(ex);
            }
        }

        // POST (zaten düzelttik, diğerleriyle tutarlı hale getirdik)
        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync(endpoint, request, ct);
                var statusCode = (int)httpResponse.StatusCode;
                var contentString = await httpResponse.Content.ReadAsStringAsync(ct);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<TResponse>>(JsonOptions, ct);

                    if (apiResponse == null)
                    {
                        return ApiResponse<TResponse>.Failure(statusCode, $"Deserialize edilemedi. Ham içerik: {contentString}", null);
                    }

                    if (apiResponse.StatusCode == 0 || apiResponse.StatusCode == default)
                    {
                        apiResponse = apiResponse with { StatusCode = statusCode };
                    }

                    return apiResponse;
                }

                return await HandleErrorResponse<TResponse>(statusCode, contentString);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                return ApiResponse<TResponse>.Failure(503, "Sunucuya bağlanılamadı – internetini kontrol et ponçik 😏", null);
            }
            catch (TaskCanceledException)
            {
                return ApiResponse<TResponse>.Failure(408, "İstek zaman aşımına uğradı, yavaş yavaş geliyorsun galiba 😉", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponse>.FromException(ex);
            }
        }

        // PUT
        public async Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(
            string endpoint,
            TRequest request,
            CancellationToken ct = default)
        {
            try
            {
                var httpResponse = await _httpClient.PutAsJsonAsync(endpoint, request, ct);
                var statusCode = (int)httpResponse.StatusCode;
                var contentString = await httpResponse.Content.ReadAsStringAsync(ct);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<TResponse>>(JsonOptions, ct);

                    if (apiResponse == null)
                    {
                        return ApiResponse<TResponse>.Failure(statusCode, $"Deserialize edilemedi. Ham içerik: {contentString}", null);
                    }

                    if (apiResponse.StatusCode == 0 || apiResponse.StatusCode == default)
                    {
                        apiResponse = apiResponse with { StatusCode = statusCode };
                    }

                    return apiResponse;
                }

                return await HandleErrorResponse<TResponse>(statusCode, contentString);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                return ApiResponse<TResponse>.Failure(503, "Sunucuya bağlanılamadı – internetini kontrol et ponçik 😏", null);
            }
            catch (TaskCanceledException)
            {
                return ApiResponse<TResponse>.Failure(408, "İstek zaman aşımına uğradı, yavaş yavaş geliyorsun galiba 😉", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponse>.FromException(ex);
            }
        }

        // DELETE
        public async Task<ApiResponse<bool>> DeleteAsync(string endpoint, CancellationToken ct = default)
        {
            try
            {
                var httpResponse = await _httpClient.DeleteAsync(endpoint, ct);
                var statusCode = (int)httpResponse.StatusCode;
                var contentString = await httpResponse.Content.ReadAsStringAsync(ct);

                if (httpResponse.IsSuccessStatusCode)
                {
                    // DELETE başarılıysa genelde 204 döner ve body boş olur
                    if (statusCode == StatusCodes.Status204NoContent)
                    {
                        return ApiResponse<bool>.Success(true, statusCode);
                    }

                    var apiResponse = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<bool>>(JsonOptions, ct);

                    if (apiResponse == null)
                    {
                        return ApiResponse<bool>.Failure(statusCode, $"Deserialize edilemedi. Ham içerik: {contentString}", null);
                    }

                    if (apiResponse.StatusCode == 0 || apiResponse.StatusCode == default)
                    {
                        apiResponse = apiResponse with { StatusCode = statusCode };
                    }

                    return apiResponse;
                }

                return await HandleErrorResponse<bool>(statusCode, contentString);
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                return ApiResponse<bool>.Failure(503, "Sunucuya bağlanılamadı – internetini kontrol et ponçik 😏", null);
            }
            catch (TaskCanceledException)
            {
                return ApiResponse<bool>.Failure(408, "İstek zaman aşımına uğradı, yavaş yavaş geliyorsun galiba 😉", null);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FromException(ex);
            }
        }

        // Ortak hata işleme metodu (kod tekrarı önlemek için)
        private async Task<ApiResponse<T>> HandleErrorResponse<T>(int statusCode, string contentString)
        {
            Dictionary<string, string[]>? errors = null;

            try
            {
                var validationProblem = await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(
                    new MemoryStream(System.Text.Encoding.UTF8.GetBytes(contentString)),
                    JsonOptions);

                if (validationProblem?.Errors != null && validationProblem.Errors.Count > 0)
                {
                    errors = validationProblem.Errors.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value ?? Array.Empty<string>()
                    );
                }
            }
            catch (JsonException jsonEx)
            {
                return ApiResponse<T>.Failure(
                    statusCode,
                    $"Sunucu yanıtı JSON formatında değil veya beklenmeyen yapıya sahip. Detay: {jsonEx.Message}",
                    null);
            }
            catch (Exception ex)
            {
                return ApiResponse<T>.Failure(
                    statusCode,
                    $"Yanıt işlenirken hata oluştu: {ex.Message}",
                    null);
            }

            return ApiResponse<T>.Failure(
                statusCode,
                string.IsNullOrWhiteSpace(contentString) ? $"Sunucu hatası (HTTP {statusCode})" : contentString.Trim(),
                errors);
        }
    }
}