using AutoMapper;
using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Cache;
using DeveloperStore.Infra.Data.MongoDB.Cache;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DeveloperStore.Application.Services
{
    public class SaleAppService : ISaleAppService
    {
        private readonly ISaleCache? _saleCache;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SaleAppService(ISaleCache saleCache,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _saleCache = saleCache;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<SaleQuery> CreateAsync(SaleCreateCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SaleQuery> UpdateAsync(SaleUpdateCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SaleQuery> DeleteAsync(SaleDeleteCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SaleQuery>> GetAllAsync()
        {
            try
            {
                var sales = await _saleCache.GetAllAsync();
                return _mapper.Map<List<SaleQuery>>(sales);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SaleQuery> GetByIdAsync(string id)
        {
            try
            {
                var sale = await _saleCache.GetByIdAsync(id);
                return _mapper.Map<SaleQuery>(sale);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SaleQuery> MakeSale(SaleItemSaleCompleteCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
