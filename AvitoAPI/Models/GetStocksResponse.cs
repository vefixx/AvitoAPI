using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class GetStocksResponse
{
    [JsonProperty("stocks")]
    public List<AvitoStock> Stocks { get; set; }
}