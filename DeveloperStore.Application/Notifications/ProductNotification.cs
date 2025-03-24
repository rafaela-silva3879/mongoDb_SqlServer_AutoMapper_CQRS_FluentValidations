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
    public class ProductNotification : INotification
    {
        public Product? Product { get; set; }
        public ActionNotification? Notification { get; set; }

        public Expression<Func<Product, bool>>? Filter { get; set; } // Filtro dinâmico

        public ProductNotification(Product product, ActionNotification notification, Expression<Func<Product, bool>>? filter = null)
        {
            Product = product;
            Notification = notification;
            Filter = filter;
        }
    }
}
