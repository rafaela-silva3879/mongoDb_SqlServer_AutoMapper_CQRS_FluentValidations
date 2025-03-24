using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Repositories;
using DeveloperStore.Infra.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Infra.Data.Repository
{
    public class SaleItemRepository
    : BaseRepository<SaleItem, string>, ISaleItemRepository
    {
        private readonly DataContext? _dataContext;
        public SaleItemRepository(DataContext? dataContext)
        : base(dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
