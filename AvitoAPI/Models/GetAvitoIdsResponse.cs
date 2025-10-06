using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class GetAvitoIdsResponse
{
    [JsonProperty("items")]
    public List<AvitoAutoLoadItem> Items { get; set; }
}