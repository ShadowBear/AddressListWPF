using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressListWPF.Models;

namespace AddressListWPF.Services
{
    interface IContactInfoService
    {
        Task<List<ContactInfo>> GetContactInfosAsync();
        Task<List<ContactInfo>> GetContactInfosByFileIdAsync(int fileId);
    }
}
