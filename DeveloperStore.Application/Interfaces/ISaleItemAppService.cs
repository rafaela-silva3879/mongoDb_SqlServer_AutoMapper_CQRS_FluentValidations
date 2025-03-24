using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Interfaces
{
    public interface ISaleItemAppService
    {
        Task<SaleItemQuery> CreateAsync(SaleItemCreateCommand command);
        Task<SaleItemQuery> UpdateAsync(SaleItemUpdateCommand command);
        Task<SaleItemQuery> DeleteAsync(SaleItemDeleteCommand command);
        Task<List<SaleItemQuery>> GetAllAsync();
        Task<SaleItemQuery> GetByIdAsync(string id);
        Task<SaleItemQuery> CalculateTotalItemAmmount(SaleItemCalculateTotalItemAmountCommand command);
    }
}
