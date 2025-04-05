using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressListWPF.Models;

namespace AddressListWPF.Services
{
    public interface IAddressService
    {
        Task<List<Address>> GetAddressesAsync();
        Task<List<Address>> SearchAddressesAsync(string aktenzeichen);
        Task<bool> AddAddressAsync(Address address);
    }
}
