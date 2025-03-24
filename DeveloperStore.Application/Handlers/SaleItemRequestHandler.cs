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
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Handlers
{
    public class SaleItemRequestHandler :
        IRequestHandler<SaleItemCreateCommand, SaleItemQuery>,
        IRequestHandler<SaleItemDeleteCommand, SaleItemQuery>,
        IRequestHandler<SaleItemUpdateCommand, SaleItemQuery>,
         IRequestHandler<SaleItemCalculateTotalItemAmountCommand, SaleItemQuery>
    {
        private readonly ISaleItemDomainService _saleItemDomainService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public SaleItemRequestHandler(
            ISaleItemDomainService saleItemDomainService,
            IMapper mapper,
            IMediator mediator)
        {
            _saleItemDomainService = saleItemDomainService;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<SaleItemQuery> Handle(SaleItemCreateCommand request, CancellationToken cancellationToken)
        {
            var saleItem = _mapper.Map<SaleItem>(request);

            await _saleItemDomainService.AddAsync(saleItem).ConfigureAwait(false);

            var notification = new SaleItemNotification(saleItem, ActionNotification.AddAsync);
            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<SaleItemQuery>(saleItem);
        }

        public async Task<SaleItemQuery> Handle(SaleItemDeleteCommand request, CancellationToken cancellationToken)
        {
            var saleItem = await _saleItemDomainService.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (saleItem == null)
            {
                throw new KeyNotFoundException("Sale Item not found.");
            }

            await _saleItemDomainService.DeleteAsync(saleItem).ConfigureAwait(false);

            var notification = new SaleItemNotification(saleItem, ActionNotification.DeleteAsync);
            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<SaleItemQuery>(saleItem);
        }

        public async Task<SaleItemQuery> Handle(SaleItemUpdateCommand request, CancellationToken cancellationToken)
        {
            var saleItem = await _saleItemDomainService.GetByIdAsync(request.Id).ConfigureAwait(false);

            if (saleItem == null)
            {
                throw new KeyNotFoundException("Sale Item not found.");
            }

            saleItem.TotalItemAmount =request.TotalItemAmount;
            saleItem.Quantity = request.Quantity;
            saleItem.Discount = request.Discount;

            await _saleItemDomainService.UpdateAsync(saleItem).ConfigureAwait(false);

            var notification = new SaleItemNotification(saleItem, ActionNotification.UpdateAsync);
            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<SaleItemQuery>(saleItem);
        }

        public async Task<SaleItemQuery> Handle(SaleItemCalculateTotalItemAmountCommand? request, CancellationToken cancellationToken)
        {
            var saleItem = _mapper.Map<SaleItem>(request);

            await _saleItemDomainService.CalculateTotalItemAmountCommand(saleItem).ConfigureAwait(false);

            var notification = new SaleItemNotification(saleItem, ActionNotification.CalculateTotalItemAmount);
            await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);

            return _mapper.Map<SaleItemQuery>(saleItem);
        }
    }
}
