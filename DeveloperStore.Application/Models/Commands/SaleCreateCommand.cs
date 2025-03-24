using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Application.Handlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Commands
{
    public class SaleCreateCommand : IRequest<SaleQuery>
    {
        public DateTime SaleDate { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public bool IsCancelled { get; set; }
        public string UserId { get; set; }

    }
}
