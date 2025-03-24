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
    public class SaleNotificationHandler
 : INotificationHandler<SaleNotification>
    {
        private readonly ISaleCache? _saleCache;
        public SaleNotificationHandler(ISaleCache? saleCache)
        {
            _saleCache = saleCache;
        }

        public async Task Handle(SaleNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.Notification)
            {
                case ActionNotification.AddAsync:
                    await _saleCache.AddAsync(notification.Sale);
                    break;

                case ActionNotification.UpdateAsync:
                    await _saleCache.UpdateAsync(notification.Sale);
                    break;

                case ActionNotification.DeleteAsync:
                    await _saleCache.DeleteAsync(notification.Sale);
                    break;

                case ActionNotification.GetAllAsync:
                    await _saleCache.GetAllAsync();
                    break;

                case ActionNotification.GetByIdAsync:
                    await _saleCache.GetByIdAsync(notification.Sale.Id);
                    break;

                case ActionNotification.GetAllWithFilterAsync:
                    if (notification.Filter != null)
                        await _saleCache.GetAllWithFilterAsync(notification.Filter);
                    break;

                case ActionNotification.GetWithFilterAsync:
                    if (notification.Filter != null)
                        await _saleCache.GetWithFilterAsync(notification.Filter);
                    break;
            }
        }
    }
}
