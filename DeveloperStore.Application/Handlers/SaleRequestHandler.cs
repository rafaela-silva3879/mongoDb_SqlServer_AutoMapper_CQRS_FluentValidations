using AutoMapper;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Application.Notifications;
using DeveloperStore.Domain.Entities.Enums;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using DeveloperStore.Domain.Services;
using DeveloperStore.Domain.Interfaces.Cache;
namespace DeveloperStore.Application.Handlers
{
    public class SaleRequestHandler :
        IRequestHandler<SaleCreateCommand, SaleQuery>,
        IRequestHandler<SaleDeleteCommand, SaleQuery>,
        IRequestHandler<SaleUpdateCommand, SaleQuery>,
        IRequestHandler<SaleItemSaleCompleteCommand, SaleQuery>
    {
        private readonly ISaleDomainService? _saleDomainService;
        private readonly ISaleItemDomainService? _saleItemDomainService;
        private readonly IProductDomainService _productDomainService;
        private readonly ISaleCache _saleCache;
        private readonly IMapper _mapper;
        private readonly IMediator? _mediator;

        public SaleRequestHandler(
            ISaleDomainService? saleDomainService,
            ISaleItemDomainService? saleItemDomainService,
            IProductDomainService? productDomainService,
            ISaleCache saleCache,           
            IMapper mapper,
            IMediator? mediator)
        {
            _saleDomainService = saleDomainService;
            _saleItemDomainService = saleItemDomainService;
            _productDomainService = productDomainService;
            _saleCache = saleCache;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<SaleQuery> Handle(SaleItemSaleCompleteCommand request, CancellationToken cancellationToken)
        {
            var saleItemList = new List<SaleItem>();

            foreach (var item in request.SaleItems)
            {
                var saleItem = new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };
                saleItemList.Add(saleItem);
            }

            var sale = await _saleDomainService.MakeSale(saleItemList, request.UserId);

            var notification = new SaleNotification(sale, ActionNotification.AddAsync);
            await _mediator.Publish(notification);

            //transformar em uma query:
            var query = new SaleQuery();
            query.TotalSaleAmount = sale.TotalSaleAmount;
            query.SaleDate = sale.SaleDate;
            query.IsCancelled = sale.IsCancelled;
            query.Id = sale.Id;
            query.UserId = sale.UserId;
            query.User = new UserQuery();
            query.User.Name = sale.User.Name;
            query.User.Email = sale.User.Email;
            query.User.UserProfile = sale.User.UserProfile;
            query.SaleItems = new List<SaleItemQuery>();
            foreach (var item in saleItemList)
            {
                var s = new SaleItemQuery();
                s.Quantity = item.Quantity;
                s.Discount = item.Discount;
                s.SaleId = item.SaleId;
                s.Id = item.Id;
                s.TotalItemAmount = item.TotalItemAmount;
                s.ProductId = item.ProductId;
                s.Product = new ProductQuery();
                var p = new Product();
                p=await _productDomainService.GetByIdAsync(item.ProductId);
                s.Product.Name = p.Name;
                s.Product.UnitPrice = p.UnitPrice;
                s.Product.Quantity = p.Quantity;
                s.Product.UserId = p.UserId;
          
                query.SaleItems.Add(s);
            }
            return query;
        }



        public async Task<SaleQuery> Handle(SaleCreateCommand request, CancellationToken cancellationToken)
        {
            var sale = _mapper.Map<Sale>(request);

            await _saleDomainService.AddAsync(sale);
            await _saleCache.AddAsync(sale);

            var notification = new SaleNotification(
                sale,
                ActionNotification.AddAsync
            );

            await _mediator.Publish(notification);

            var query = _mapper.Map<SaleQuery>(sale);

            return query;
        }

        public async Task<SaleQuery> Handle(SaleDeleteCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleDomainService.GetByIdAsync(request.Id);

            if (sale == null)
            {
                throw new KeyNotFoundException("Sale not found.");

            }
            await _saleDomainService.DeleteAsync(sale);
            await _saleCache.DeleteAsync(sale);

            var notification = new SaleNotification(
                    sale,
                    ActionNotification.DeleteAsync
                );

            await _mediator.Publish(notification);

            var query = _mapper.Map<SaleQuery>(sale);

            return query;
        }

        public async Task<SaleQuery> Handle(SaleUpdateCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleDomainService.GetByIdAsync(request.Id);

            if (sale == null)
            {
                throw new KeyNotFoundException("Sale not found.");
            }

            sale.SaleDate = request.SaleDate;
            sale.IsCancelled = request.IsCancelled;
            sale.TotalSaleAmount = request.TotalSaleAmount;

            await _saleDomainService.UpdateAsync(sale);
            await _saleCache.UpdateAsync(sale);
            var notification = new SaleNotification(
                sale,
                ActionNotification.UpdateAsync
            );

            await _mediator.Publish(notification);

            var query = _mapper.Map<SaleQuery>(sale);

            return query;
        }
    }
}
