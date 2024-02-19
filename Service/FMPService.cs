using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace api.Service
{
    public sealed class FMPService : IFMPService
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration config;

        public FMPService(HttpClient httpClient, IConfiguration config)
        {
            this.httpClient = httpClient;
            this.config = config;
        }

        public async Task<Stock?> FindStockBySymbol(string symbol)
        {
            try
            {
                var result = await httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={config["FMPKey"]}");

                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks[0];

                    if (stock != null)
                    {
                        return stock.ToStockDTO();
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
