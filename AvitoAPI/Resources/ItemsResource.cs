using System.Globalization;
using AvitoAPI.Enums;
using AvitoAPI.Models;

namespace AvitoAPI.Resources;

public class ItemsResource : ResourceBase
{
    public ItemsResource(AvitoClient client) : base(client)
    {
        
    }
    
    /// <summary>
    /// Возвращает список объявлений авторизованного пользователя - статус, категорию и ссылку на сайте
    /// https://developers.avito.ru/api-catalog/item/documentation#operation/getItemsInfo
    /// </summary>
    /// <param name="category">Идентификатор категории объявления</param>
    /// <param name="updatedAtFrom">Фильтр больше либо равно по дате обновления/редактирования объявления</param>
    /// <param name="statuses">Статус объявления на сайте (можно указать несколько значений)</param>
    /// <param name="page">Номер страницы (целое число больше 0)</param>
    /// <param name="perPage">Количество записей на странице (целое число больше 0 и меньше 100)</param>
    /// <returns></returns>
    public async Task<List<AvitoItem>> GetItemsInfoAsync(int? category = null, DateTime? updatedAtFrom = null, AvitoItemStatus[]? statuses = null, int page = 1, int perPage = 99)
    {
        if (statuses == null)
        {
            statuses = [AvitoItemStatus.Active];
        }

        Dictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("per_page", perPage.ToString());
        query.Add("page", page.ToString());
        query.Add("status", string.Join(",", statuses).ToLower());
        if (updatedAtFrom != null)
            query.Add("updatedAtFrom", ((DateTime)updatedAtFrom).ToString("yyyy-MM-dd"));
        if (category != null)
            query.Add("category", category.ToString());
        
        var response = await _client.GetAsync<GetItemsResponse>("core/v1/items", queryParams: query);
        return response.Resources;
    }
    
    /// <summary>
    /// Обновляет цену товара по id. Максимальное количество запросов в минуту - 150
    /// https://developers.avito.ru/api-catalog/item/documentation#operation/updatePrice
    /// </summary>
    /// <param name="itemId">Идентификатор объявления на сайте</param>
    /// <param name="price">Цена</param>
    /// <returns></returns>
    public async Task<bool> UpdatePriceAsync(ulong itemId, int price)
    {
        Dictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("price", price.ToString());
        
        var response = await _client.GetAsync<UpdatePriceResponse>($"core/v1/items/{itemId}/update_price", queryParams: query);
        return response.Result.Success;
    }
}