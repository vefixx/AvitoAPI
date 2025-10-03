using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AvitoAPI.Exceptions;
using AvitoAPI.Models;
using AvitoAPI.Resources;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;

namespace AvitoAPI;

public class AvitoClient
{
    public const string BaseUrl = "https://api.avito.ru/";

    private static string _accessToken = string.Empty;
    private string _clientId;
    private string _clientSecret;

    private const int MaxRetries = 2;
    private const short AttemptsDelayMs = 500;

    private HttpClient _httpClient;
    
    // Resources
    public ItemsResource ItemsResource { get; }
    public StockManagementResource StockManagementResource { get; }
    public AutoLoadResource AutoLoadResource { get; }

    public AvitoClient(string clientId, string clientSecret)
    {
        _httpClient = new HttpClient();
        _clientId = clientId;
        _clientSecret = clientSecret;

        ItemsResource = new ItemsResource(this);
        StockManagementResource = new StockManagementResource(this);
        AutoLoadResource = new AutoLoadResource(this);
    }
    
    /// <summary>
    /// Инициализирует токен по <see cref="_clientId"/> и <see cref="_clientSecret"/>.
    /// </summary>
    /// <param name="tokenExpires"></param>
    /// <exception cref="TokenInitializeException">Ошибка при преобразовании ответа - токен не распознан</exception>
    private async Task TryInitializeTokenAsync(bool tokenExpires = false)
    {
        if (tokenExpires || string.IsNullOrEmpty(_accessToken))
        {
            string uri = BaseUrl + $"token";

            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["grant_type"] = "client_credentials",
            };

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
                { Content = new FormUrlEncodedContent(data) };
            using var responseMessage = await _httpClient.SendAsync(requestMessage);

            string content = await responseMessage.Content.ReadAsStringAsync();
            AvitoTokenResponse? tokenResponse = JsonConvert.DeserializeObject<AvitoTokenResponse>(content);

            if (tokenResponse == null)
            {
                throw new TokenInitializeException($"Не удалось получить токен. STATUS={responseMessage.StatusCode}, CONTENT={content}");
            }
            
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");
            _accessToken = tokenResponse.AccessToken;
        }
    }

    private async Task<T> MakeRequestAsync<T>(HttpMethod method, string endpoint, object? data = null, Dictionary<string, string?>? queryParams = null)
    {
        string url = BaseUrl + endpoint;
        
        HttpStatusCode? lastStatus = null;
        for (int i = 0; i < MaxRetries; i++)
        {
            string uri = queryParams is not null ? QueryHelpers.AddQueryString(url, queryParams) : url;
            
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
        
            HttpResponseMessage response;
            if (method == HttpMethod.Post)
                response = await _httpClient.PostAsJsonAsync(uri, data, jsonOptions);
            else if (method == HttpMethod.Get)
                response = await _httpClient.GetAsync(uri);
            else if (method == HttpMethod.Put)
                response = await _httpClient.PutAsJsonAsync(uri, data, jsonOptions);
            else
                throw new Exception($"Неизвестный тип запроса {method.Method}");
            
            string content = await response.Content.ReadAsStringAsync();

            lastStatus = response.StatusCode;

            switch (lastStatus)
            {
                case HttpStatusCode.Unauthorized:
                    await TryInitializeTokenAsync();
                    continue;
                case HttpStatusCode.BadRequest:
                    throw new HttpRequestException($"Ошибка запроса STATUS={response.StatusCode}, CONTENT={content}");
                case HttpStatusCode.TooManyRequests:
                    goto case HttpStatusCode.BadRequest;
                case HttpStatusCode.InternalServerError:
                    goto case HttpStatusCode.BadRequest;
            }
            
            var result = JsonConvert.DeserializeObject<T>(content);

            if (result == null)
                throw new JsonException($"Ошибка преобразования запроса STATUS={response.StatusCode}, CONTENT={content}");
            
            return result;
        }
        
        throw new AttemptsLimitException($"Превышен лимит попыток ({MaxRetries}); LAST_STATUS={lastStatus}");
    }
    
    /// <summary>
    /// Выполняет POST запрос на эндпоинт <paramref name="endpoint"/>.
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="data"></param>
    /// <param name="queryParams"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Возвращает объект запроса</returns>
    /// <exception cref="HttpRequestException">Сервер вернул статус, отличимый от 2xx</exception>
    /// <exception cref="JsonException">Ошибка преобразования JSON ответа</exception>
    /// <exception cref="AttemptsLimitException">Превышен лимит попыток запросов</exception>
    /// /// <exception cref="TokenInitializeException">Ошибка при преобразовании ответа получения токена - токен не распознан</exception>
    public async Task<T> PostAsync<T>(string endpoint, object? data = null, Dictionary<string, string?>? queryParams = null)
    {
        return await MakeRequestAsync<T>(HttpMethod.Post, endpoint, data, queryParams);
    }
    
    /// <summary>
    /// Выполняет GET запрос на эндпоинт <paramref name="endpoint"/>.
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="data"></param>
    /// <param name="queryParams"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Возвращает объект запроса</returns>
    /// <exception cref="HttpRequestException">Сервер вернул статус, отличимый от 2xx</exception>
    /// <exception cref="JsonException">Ошибка преобразования JSON ответа</exception>
    /// <exception cref="AttemptsLimitException">Превышен лимит попыток запросов</exception>
    /// /// <exception cref="TokenInitializeException">Ошибка при преобразовании ответа получения токена - токен не распознан</exception>
    public async Task<T> GetAsync<T>(string endpoint, object? data = null, Dictionary<string, string?>? queryParams = null)
    {
        return await MakeRequestAsync<T>(HttpMethod.Get, endpoint, data, queryParams);
    }
    
    /// <summary>
    /// Выполняет PUT запрос на эндпоинт <paramref name="endpoint"/>.
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="data"></param>
    /// <param name="queryParams"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Возвращает объект запроса</returns>
    /// <exception cref="HttpRequestException">Сервер вернул статус, отличимый от 2xx</exception>
    /// <exception cref="JsonException">Ошибка преобразования JSON ответа</exception>
    /// <exception cref="AttemptsLimitException">Превышен лимит попыток запросов</exception>
    /// /// <exception cref="TokenInitializeException">Ошибка при преобразовании ответа получения токена - токен не распознан</exception>
    public async Task<T> PutAsync<T>(string endpoint, object? data = null, Dictionary<string, string?>? queryParams = null)
    {
        return await MakeRequestAsync<T>(HttpMethod.Put, endpoint, data, queryParams);
    }
}