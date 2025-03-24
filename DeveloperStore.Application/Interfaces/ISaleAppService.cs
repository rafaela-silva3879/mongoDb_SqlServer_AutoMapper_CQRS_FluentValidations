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
    public interface ISaleAppService
    {
        Task<SaleQuery> CreateAsync(SaleCreateCommand command);
        Task<SaleQuery> UpdateAsync(SaleUpdateCommand command);
        Task<SaleQuery> DeleteAsync(SaleDeleteCommand command);
        Task<List<SaleQuery>> GetAllAsync();
        Task<SaleQuery> GetByIdAsync(string id);
        Task<SaleQuery> MakeSale(SaleItemSaleCompleteCommand commandList);

    }
}
