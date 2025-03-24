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
    public class UserNotificationHandler
 : INotificationHandler<UserNotification>
    {
        private readonly IUserCache? _userCache;
        public UserNotificationHandler(IUserCache? userCache)
        {
            _userCache = userCache;
        }

        public async Task Handle(UserNotification notification, CancellationToken cancellationToken)
        {
            switch (notification.Notification)
            {
                case ActionNotification.AddAsync:
                    await _userCache.AddAsync(notification.User);
                    break;

                case ActionNotification.UpdateAsync:
                    await _userCache.UpdateAsync(notification.User);
                    break;

                case ActionNotification.DeleteAsync:
                    await _userCache.DeleteAsync(notification.User);
                    break;

                case ActionNotification.GetAllAsync:
                    await _userCache.GetAllAsync();
                    break;

                case ActionNotification.GetByIdAsync:
                    await _userCache.GetByIdAsync(notification.User.Id);
                    break;

                case ActionNotification.GetAllWithFilterAsync:                   
                    if (notification.Filter != null)
                        await _userCache.GetAllWithFilterAsync(notification.Filter);
                    break;

                case ActionNotification.GetWithFilterAsync:
                    if (notification.Filter != null)
                        await _userCache.GetWithFilterAsync(notification.Filter);
                    break;
            }
        }
    }
}
