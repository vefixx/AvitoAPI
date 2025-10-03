using Newtonsoft.Json;

namespace AvitoAPI.Models;


/// <summary>
/// Модель, которая хранит в себе данные об статусе обновления остатка
/// </summary>
public class AvitoUpdatedStockResult
{
    [JsonProperty("errors")]
    public string[] Errors { get; set; }
    
    [JsonProperty("external_id")]
    public string? ExternalId { get; set; }
    
    [JsonProperty("item_id")]
    public ulong ItemId { get; set; }
    
    [JsonProperty("success")]
    public bool Success { get; set; }
}