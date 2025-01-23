using System.Net.Http.Json;
using StockTicker.Models;

namespace StockTicker.Services;

public class StockService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    
    public StockService(string apiKey)
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };
        _apiKey = apiKey;
    }
    
    public async Task<Stock> GetStockPrice(string symbol)
    {
        var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";
        
        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var data = await response.Content.ReadFromJsonAsync<Dictionary<string, Dictionary<string, string>>>();
        
        if (data == null || !data.ContainsKey("Global Quote"))
            throw new Exception($"Invalid response for symbol {symbol}");
            
        var quote = data["Global Quote"];
        var price = decimal.Parse(quote["05. price"]);
        
        return new Stock(symbol, price);
    }
} 