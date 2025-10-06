using System.Globalization;
using AvitoAPI.Enums;
using AvitoAPI.Models;

namespace AvitoAPI.Resources;

public class ItemsResource : ResourceBase
{
    public ItemsResource(AvitoClient client) : base(client)
    {
        
    }

    public async Task<List<AvitoItem>> GetItemsAsync(int? category = null, DateTime? updatedAtFrom = null, AvitoItemStatus[]? statuses = null, int page = 1, int perPage = 99)
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

    public async Task<bool> UpdatePriceAsync(ulong itemId, int price)
    {
        Dictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("price", price.ToString());
        
        var response = await _client.GetAsync<UpdatePriceResponse>($"core/v1/items/{itemId}/update_price", queryParams: query);
        return response.Result.Success;
    }
}