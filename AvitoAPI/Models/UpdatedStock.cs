using Newtonsoft.Json;

namespace AvitoAPI.Models;


/// <summary>
/// Модель обновленного остатка, который будет передан в запрос на обновление остатков
/// </summary>
public class UpdatedStock
{
    public string ExternalId { get; set; }
    public required ulong ItemId { get; set; }
    public required ulong Quantity { get; set; }
}