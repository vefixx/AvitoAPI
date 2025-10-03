namespace AvitoAPI.Resources;

public abstract class ResourceBase
{
    protected AvitoClient _client;
    public ResourceBase(AvitoClient client)
    {
        _client = client;
    }
}