using AvitoAPI.Models;

namespace AvitoAPI.Resources;

public class AutoLoadResource: ResourceBase
{
    public AutoLoadResource(AvitoClient client) : base(client)
    {
        
    }
    
    /// <summary>
    /// Метод позволяет получить идентификаторы (ID) объявлений из файла автозагрузки по ID объявлений на Авито.
    /// https://developers.avito.ru/api-catalog/autoload/documentation#operation/getAdIdsByAvitoIds
    /// </summary>
    /// <param name="avitoIds">Список ID объявлений.</param>
    /// <returns></returns>
    public async Task<List<AvitoAutoLoadItem>> GetAdIdsByAvitoIdsAsync(ulong[] avitoIds)
    {
        Dictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("query", string.Join(",", avitoIds));
        
        var response = await _client.GetAsync<GetAvitoIdsResponse>("autoload/v2/items/ad_ids", queryParams: query);
        return response.Items;
    }
    
    /// <summary>
    /// Метод позволяет получить идентификаторы (ID) объявлений на Авито по идентификаторам объявлений из файла автозагрузки.
    /// Список ID объявлений.
    /// https://developers.avito.ru/api-catalog/autoload/documentation#operation/getAvitoIdsByAdIds
    /// </summary>
    /// <param name="adIds">Список ID объявлений. Список с идентификаторами объявлений из файла</param>
    /// <returns></returns>
    public async Task<List<AvitoAutoLoadItem>> GetAvitoIdsByAdIdsAsync(string[] adIds)
    {
        Dictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("query", string.Join(",", adIds));
        
        var response = await _client.GetAsync<GetAvitoIdsResponse>("autoload/v2/items/avito_ids", queryParams: query);
        return response.Items;
    }
}