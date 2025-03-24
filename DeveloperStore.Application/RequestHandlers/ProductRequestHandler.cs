using AutoMapper;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Application.Notifications;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Services;
using MediatR;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperStore.Application.RequestHandlers
{
    public class ProductRequestHandler :
        IRequestHandler<ProductCreateCommand, ProductQuery>,
        IRequestHandler<ProductDeleteCommand, ProductQuery>,
        IRequestHandler<ProductUpdateCommand, ProductQuery>,
       IRequestHandler<ProductGetByIdCommand, ProductQuery>
    {
        private readonly IProductDomainService _productDomainService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ProductRequestHandler(
            IProductDomainService productDomainService,
            IMapper mapper,
            IMediator mediator)
        {
            _productDomainService = productDomainService;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ProductQuery> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Photo = request.Photo,
                UnitPrice = request.UnitPrice,
            };

            await _productDomainService.AddAsync(product);

            await _mediator.Publish(new ProductNotification(product, ActionNotification.AddAsync));

            var query = _mapper.Map<ProductQuery>(product);

            return query;
        }

        public async Task<ProductQuery> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var product = await _productDomainService.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new Exception("Product not registered.");
            }

            await _productDomainService.DeleteAsync(product);

            await _mediator.Publish(new ProductNotification(product, ActionNotification.DeleteAsync));

            var productQuery = _mapper.Map<ProductQuery>(product);

            return productQuery;
        }

        public async Task<ProductQuery> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _productDomainService.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            product.UnitPrice = request.UnitPrice;
            product.Name = request.Name;
            product.Photo = request.Photo;

            await _productDomainService.UpdateAsync(product);

            var notification = new ProductNotification(
                product,
                ActionNotification.UpdateAsync
            );

            var query = _mapper.Map<ProductQuery>(product);

            return query;
        }

        public async Task<ProductQuery> Handle(ProductGetByIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _productDomainService.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            var query = _mapper.Map<ProductQuery>(product);

            return query;
        }
    }
}