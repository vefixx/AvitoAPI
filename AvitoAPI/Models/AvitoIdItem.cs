using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class AvitoIdItem
{
    [JsonProperty("ad_id")]
    public string AdId { get; set; }
    [JsonProperty("avito_id")]
    public long? AvitoId { get; set; }
}