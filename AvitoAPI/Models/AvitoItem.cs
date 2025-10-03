using System.Text.Json.Serialization;
using AvitoAPI.Enums;
using Newtonsoft.Json;

namespace AvitoAPI.Models;


/// <summary>
/// Модель объявления
/// </summary>
public class AvitoItem
{
    [JsonProperty("address")]
    public string Address { get; set; }
    
    [JsonProperty("category")]
    public AvitoCategory Category { get; set; }
    
    [JsonProperty("id")]
    public ulong Id { get; set; }
    
    [JsonProperty("price")]
    public int? Price { get; set; }
    
    [JsonProperty("status")]
    public AvitoItemStatus Status { get; set; }
    
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("url")]
    public string? Url { get; set; }
}