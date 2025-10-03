using AvitoAPI.Models;

namespace AvitoAPI.Resources;

public class StockManagementResource : ResourceBase
{
    public StockManagementResource(AvitoClient client) : base(client)
    {
    }

    public async Task<List<AvitoStock>> GetStocksAsync(ulong[] itemIds, bool strongConsistency = false)
    {
        var response = await _client.PostAsync<GetStocksResponse>("stock-management/1/info",
            data: new { itemIds, strongConsistency });
        return response.Stocks;
    }
    
    /// <summary>
    /// Отправляет запрос на обновление остатков
    /// </summary>
    /// <param name="stocks">Список объектов <see cref="UpdatedStock"/>. Переданные остатки будут обновлены согласно указанным значениям в них.</param>
    /// <returns>Список остатков, которые были обновлены (успешно/неуспешно)</returns>
    public async Task<List<AvitoUpdatedStockResult>> UpdateStocksAsync(UpdatedStock[] stocks)
    {
        var response = await _client.PutAsync<UpdateStocksResponse>("stock-management/1/stocks",
            data: new { Stocks = stocks });
        return response.Stocks;
    }
}