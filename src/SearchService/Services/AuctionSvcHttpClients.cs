using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    public class AuctionSvcHttpClients
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuctionSvcHttpClients(HttpClient httpClient, IConfiguration config)
        {
           _httpClient = httpClient;
           _config = config;
        }
        public async Task<List<Item>> GetItemsForSearchDb()
        {
            var lastUpdate = await DB.Find<Item, string>()
                .Sort(x => x.Descending(i => i.UpdatedAt))
                .Project(x => x.UpdatedAt.ToString())
                .ExecuteFirstAsync();
            return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] + $"/api/auctions?date=" + lastUpdate);
        }
    }
}