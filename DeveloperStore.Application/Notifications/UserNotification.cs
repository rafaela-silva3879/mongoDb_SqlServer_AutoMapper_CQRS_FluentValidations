using DeveloperStore.Domain.Entities;
using MediatR;
using System;
using System.Linq.Expressions;

namespace DeveloperStore.Application.Notifications
{
    public class UserNotification : INotification
    {
        public User User { get; set; }
        public ActionNotification Notification { get; set; }
        public Expression<Func<User, bool>>? Filter { get; set; } // Filtro dinâmico

        public UserNotification(User user, ActionNotification notification, Expression<Func<User, bool>>? filter = null)
        {
            User = user;
            Notification = notification;
            Filter = filter;
        }
    }
}
