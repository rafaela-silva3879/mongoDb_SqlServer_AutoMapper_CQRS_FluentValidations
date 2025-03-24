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
    public class SaleItemNotification : INotification
    {
        public SaleItem? SaleItem { get; set; }
        public ActionNotification? Notification { get; set; }

        public Expression<Func<SaleItem, bool>>? Filter { get; set; } // Filtro dinâmico

        public SaleItemNotification(SaleItem saleItem, ActionNotification notification, Expression<Func<SaleItem, bool>>? filter = null)
        {
            SaleItem = saleItem;
            Notification = notification;
            Filter = filter;
        }
    }
}
