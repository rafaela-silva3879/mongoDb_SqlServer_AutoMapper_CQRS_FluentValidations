using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Queries
{
    public class SaleItemQuery
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }

        //Relacionamentos
        public string ProductId { get; set; }
        public ProductQuery Product { get; set; }

        public string SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
