using AvitoAPI.Models;

namespace AvitoAPI.Resources;

public class AutoLoadResource: ResourceBase
{
    public AutoLoadResource(AvitoClient client) : base(client)
    {
        
    }

    public async Task<List<AvitoAutoLoadItem>> GetAutoLoadItemsAsync(ulong[] adIds)
    {
        Dictionary<string, string?> query = new Dictionary<string, string?>();
        query.Add("query", string.Join(",", adIds));
        
        var response = await _client.GetAsync<GetAvitoIdsResponse>("autoload/v2/items/ad_ids", queryParams: query);
        return response.Items;
    }
}