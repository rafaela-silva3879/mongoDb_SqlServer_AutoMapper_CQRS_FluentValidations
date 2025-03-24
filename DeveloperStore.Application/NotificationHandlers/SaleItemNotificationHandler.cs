using DeveloperStore.Application.Notifications;
using DeveloperStore.Domain.Interfaces.Cache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.NotificationHandlers
{
    public class SaleItemNotificationHandler
 : INotificationHandler<SaleItemNotification>
    {
        private readonly ISaleItemCache? _saleItemCache;
        public SaleItemNotificationHandler(ISaleItemCache? saleItemCache)
        {
            _saleItemCache = saleItemCache;
        }

        public async Task Handle(SaleItemNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.Notification)
            {
                case ActionNotification.AddAsync:
                    await _saleItemCache.AddAsync(notification.SaleItem);
                    break;

                case ActionNotification.UpdateAsync:
                    await _saleItemCache.UpdateAsync(notification.SaleItem);
                    break;

                case ActionNotification.DeleteAsync:
                    await _saleItemCache.DeleteAsync(notification.SaleItem);
                    break;

                case ActionNotification.GetAllAsync:
                    await _saleItemCache.GetAllAsync();
                    break;

                case ActionNotification.GetByIdAsync:
                    await _saleItemCache.GetByIdAsync(notification.SaleItem.Id);
                    break;

                case ActionNotification.GetAllWithFilterAsync:
                    if (notification.Filter != null)
                        await _saleItemCache.GetAllWithFilterAsync(notification.Filter);
                    break;

                case ActionNotification.GetWithFilterAsync:
                    if (notification.Filter != null)
                        await _saleItemCache.GetWithFilterAsync(notification.Filter);
                    break;
            }
        }
    }
}
