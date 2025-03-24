using DeveloperStore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Notifications
{
    public class SaleNotification : INotification
    {
        public Sale? Sale { get; set; }
        public ActionNotification? Notification { get; set; }

        public Expression<Func<Sale, bool>>? Filter { get; set; } // Filtro dinâmico

        public SaleNotification(Sale sale, ActionNotification notification, Expression<Func<Sale, bool>>? filter = null)
        {
            Sale = sale;
            Notification = notification;
            Filter = filter;
        }
    }
}
