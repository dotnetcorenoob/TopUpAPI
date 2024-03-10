using Newtonsoft.Json;
using topUpApi.Entities;
using Microsoft.Extensions.Configuration;

namespace topUpApi.Services
{
    // IBalanceService.cs
    public interface IBalanceService
    {
        public  Task<decimal> GetBalance(int userId);

        public Task<bool> Debit(decimal amount,int userId);

        public  Task<bool> Credit(decimal amount, int userId);// Replace 'string' with the actual user identifier type
    }

    // ExternalBalanceService.cs
    public class ExternalBalanceService : IBalanceService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ExternalBalanceService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(configuration["ExternalApiUrls:BaseAddress"]);
            
        }

        public async Task<decimal> GetBalance(int userId)
        {
            var response = await _httpClient.GetAsync(_configuration["ExternalApiUrls:BalanceEndpoint"] + userId);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var balance = JsonConvert.DeserializeObject<BalanceModel>(content);

            return balance.Amount;
        }

        public async Task<bool> Debit(decimal amount, int userId)
        {
            var debitData = new { Amount = amount };
            var response = await _httpClient.PostAsJsonAsync(_configuration["ExternalApiUrls:DebitEndpoint"] + userId, debitData);
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> Credit(decimal amount, int userId)
        {
            var creditData = new { Amount = amount };
            var response = await _httpClient.PostAsJsonAsync(_configuration["ExternalApiUrls:CreditEndpoint"], creditData);
            response.EnsureSuccessStatusCode();

            return true;
        }
    }

}
