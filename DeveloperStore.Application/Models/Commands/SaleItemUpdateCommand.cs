﻿using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Application.Handlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Commands
{
    public class SaleItemUpdateCommand : IRequest<SaleItemQuery>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public decimal TotalItemAmount { get; set; }

    }
}
