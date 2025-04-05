using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AddressListWPF.Models;
using Newtonsoft.Json;

namespace AddressListWPF.Services
{
    class ContactInfoService : IContactInfoService
    {
        private readonly HttpClient _httpClient;

        public ContactInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ContactInfo>> GetContactInfosAsync()
        {
            var response = await _httpClient.GetStringAsync("contactinfo");
            return JsonConvert.DeserializeObject<List<ContactInfo>>(response) ?? new List<ContactInfo>();
        }

        public async Task<List<ContactInfo>> GetContactInfosByFileIdAsync(int fileId)
        {
            var response = await _httpClient.GetStringAsync($"contactinfo/byfileid?fileId={fileId}");
            return JsonConvert.DeserializeObject<List<ContactInfo>>(response) ?? new List<ContactInfo>();
        }
    }
}
