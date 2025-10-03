using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class UpdateStocksResponse
{
    [JsonProperty("stocks")]
    public List<AvitoUpdatedStockResult> Stocks { get; set; }
}