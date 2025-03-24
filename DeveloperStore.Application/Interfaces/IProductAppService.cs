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
    public interface IProductAppService
    {
        Task<ProductQuery> CreateAsync(ProductCreateCommand command);
        Task<ProductQuery> UpdateAsync(ProductUpdateCommand command);
        Task<ProductQuery> DeleteAsync(ProductDeleteCommand command);
        Task<List<ProductQuery>> GetAllAsync();
        Task<ProductQuery> GetByIdAsync(string id);
    }
}
