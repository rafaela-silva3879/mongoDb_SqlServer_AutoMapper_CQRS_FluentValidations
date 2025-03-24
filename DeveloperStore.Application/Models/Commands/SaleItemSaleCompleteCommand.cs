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
    public class SaleItemSaleCompleteCommand : IRequest<SaleQuery>
    {
        public List<SaleItemCommand> SaleItems { get; set; } // Uma lista de SaleItemCommand
        public string UserId { get; set; }

    }
}
