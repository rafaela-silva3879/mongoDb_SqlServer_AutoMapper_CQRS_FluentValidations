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
    public class ProductNotificationHandler
 : INotificationHandler<ProductNotification>
    {
        private readonly IProductCache? _productCache;
        public ProductNotificationHandler(IProductCache? productCache)
        {
            _productCache = productCache;
        }

        public async Task Handle(ProductNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.Notification)
            {
                case ActionNotification.AddAsync:
                    await _productCache.AddAsync(notification.Product);
                    break;

                case ActionNotification.UpdateAsync:
                    await _productCache.UpdateAsync(notification.Product);
                    break;

                case ActionNotification.DeleteAsync:
                    await _productCache.DeleteAsync(notification.Product);
                    break;

                case ActionNotification.GetAllAsync:
                    await _productCache.GetAllAsync();
                    break;

                case ActionNotification.GetByIdAsync:
                    await _productCache.GetByIdAsync(notification.Product.Id);
                    break;

                case ActionNotification.GetAllWithFilterAsync:
                    if (notification.Filter != null)
                        await _productCache.GetAllWithFilterAsync(notification.Filter);
                    break;

                case ActionNotification.GetWithFilterAsync:
                    if (notification.Filter != null)
                        await _productCache.GetWithFilterAsync(notification.Filter);
                    break;
            }
        }
    }
}
