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

namespace DeveloperStore.Application.Services
{
    public class SaleItemAppService : ISaleItemAppService
    {
        private readonly ISaleItemCache? _saleItemCache;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SaleItemAppService(ISaleItemCache saleItemCache,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _saleItemCache = saleItemCache;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<SaleItemQuery> CreateAsync(SaleItemCreateCommand command)
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

        public async Task<SaleItemQuery> UpdateAsync(SaleItemUpdateCommand command)
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

        public async Task<SaleItemQuery> DeleteAsync(SaleItemDeleteCommand command)
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

        public async Task<List<SaleItemQuery>> GetAllAsync()
        {
            try
            {
                var saleItems = await _saleItemCache.GetAllAsync();
                return _mapper.Map<List<SaleItemQuery>>(saleItems);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SaleItemQuery> GetByIdAsync(string id)
        {
            try
            {
                var saleItem = await _saleItemCache.GetByIdAsync(id);
                return _mapper.Map<SaleItemQuery>(saleItem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SaleItemQuery> CalculateTotalItemAmmount(SaleItemCalculateTotalItemAmountCommand command)
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
