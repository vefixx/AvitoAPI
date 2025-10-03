using Newtonsoft.Json;

namespace AvitoAPI.Exceptions;

public class ResponseErrorMessage
{
    [JsonProperty("message")]
    public string Message { get; set; }
}