using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AddressList.Shared.Models;
using AddressList.Shared.Services;
using System.Net.Http.Json;

namespace AddressListWPF.Services
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;

        public AddressService(HttpClient httpClient)
        {
            _httpClient = httpClient; 
        }

        public async Task<List<Address>> GetAddressesAsync()
        {
            var response = await _httpClient.GetStringAsync("address");
            return JsonConvert.DeserializeObject<List<Address>>(response) ?? new List<Address>();
        }

        public async Task<List<Address>> SearchAddressesAsync(string aktenzeichen)
        {
            var response = await _httpClient.GetAsync($"address/search?aktenzeichen={aktenzeichen}");
            return await response.Content.ReadFromJsonAsync<List<Address>>() ?? new List<Address>();
        }

        public async Task<bool> SaveAddress(Address address)
        {
            var json = JsonConvert.SerializeObject(address);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsJsonAsync("address", address, new CancellationToken());
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(errorContent);
            return response.IsSuccessStatusCode;
        }

         public Task ClearAddressData()
        {
            throw new NotImplementedException();
        }
    }
}
