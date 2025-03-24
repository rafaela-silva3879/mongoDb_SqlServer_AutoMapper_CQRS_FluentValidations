using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Commands
{
    public class ProductCreateCommand : IRequest<ProductQuery>
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Photo { get; set; }
        public string Quantity { get; set; }
    }
}
