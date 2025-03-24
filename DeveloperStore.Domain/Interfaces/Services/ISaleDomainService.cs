using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Interfaces.Services
{
    public interface ISaleDomainService : IBaseDomainService<Sale, string>
    {
        Task<Sale> MakeSale(List<SaleItem> saleItems, string userId);
    }
}
