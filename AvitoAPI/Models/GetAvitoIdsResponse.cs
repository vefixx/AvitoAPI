using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class GetAvitoIdsResponse
{
    [JsonProperty("items")]
    public List<AvitoIdItem> Items { get; set; }
}