using System.Net.Http.Json;

class Program
{
    private static readonly string API_KEY = "YOUR_API_KEY"; // Replace with your Alpha Vantage API key
    private static readonly HttpClient client = new HttpClient();
    private static readonly string[] symbols = { "MSFT", "AAPL", "GOOGL" };

    static async Task Main(string[] args)
    {
        Console.WriteLine("Simple Stock Ticker");
        Console.WriteLine("------------------");

        try
        {
            while (true)
            {
                await FetchAllStockPrices();
                await Task.Delay(30000); // Wait 30 seconds before next update
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
    }

    static async Task FetchAllStockPrices()
    {
        Console.Clear();
        Console.WriteLine($"Updating prices at {DateTime.Now}\n");

        var tasks = symbols.Select(FetchStockPrice);
        await Task.WhenAll(tasks);
    }

    static async Task FetchStockPrice(string symbol)
    {
        try
        {
            var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={API_KEY}";
            var response = await client.GetFromJsonAsync<Dictionary<string, Dictionary<string, string>>>(url);

            if (response?["Global Quote"] is Dictionary<string, string> quote)
            {
                var price = decimal.Parse(quote["05. price"]);
                Console.WriteLine($"{symbol}: ${price:F2}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching {symbol}: {ex.Message}");
        }
    }
} 