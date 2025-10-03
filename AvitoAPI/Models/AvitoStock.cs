using Newtonsoft.Json;

namespace AvitoAPI.Models;


/// <summary>
/// Модель остатка на объявлении
/// </summary>
public class AvitoStock
{
    [JsonProperty("is_multiple")]
    public bool IsMultiple { get; set; }
    
    [JsonProperty("is_out_of_stock")]
    public bool IsOutOfStock { get; set; }
    
    [JsonProperty("is_unlimited")]
    public bool IsUnlimited { get; set; }
    
    [JsonProperty("item_id")]
    public ulong ItemId { get; set; }
    
    [JsonProperty("quantity")]
    public ulong Quantity { get; set; }
}