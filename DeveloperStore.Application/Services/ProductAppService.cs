using AutoMapper;
using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Cache;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductCache? _productCache;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductAppService(IProductCache productCache,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _productCache = productCache;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ProductQuery> CreateAsync(ProductCreateCommand command)
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

        public async Task<ProductQuery> UpdateAsync(ProductUpdateCommand command)
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

        public async Task<ProductQuery> DeleteAsync(ProductDeleteCommand command)
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
        
        public async Task<List<ProductQuery>> GetAllAsync()
        {
            try
            {
                var products = await _productCache.GetAllAsync();
                return _mapper.Map<List<ProductQuery>>(products);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductQuery> GetByIdAsync(string id)
        {
            try
            {
                var product = await _productCache.GetByIdAsync(id);
                return _mapper.Map<ProductQuery>(product);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
