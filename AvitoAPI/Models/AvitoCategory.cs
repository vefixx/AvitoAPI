using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class AvitoCategory
{
    [JsonProperty("id")]
    public ulong Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
}