using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class UpdatePriceResponse
{
    [JsonProperty("result")]
    public ResultSuccessResponse Result { get; set; }
}