using Newtonsoft.Json;

namespace AvitoAPI.Models;

public class ResultSuccessResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }
}