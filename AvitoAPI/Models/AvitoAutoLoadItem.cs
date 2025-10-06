using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class AvitoAutoLoadItem
{
    [JsonProperty("ad_id")]
    public string AdId { get; set; }
    [JsonProperty("avito_id")]
    public ulong? AvitoId { get; set; }
}