using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class GetItemsResponse
{
    [JsonProperty("resources")]
    public List<AvitoItem> Resources { get; set; }
}