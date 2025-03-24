using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Queries
{
    public class SaleQuery
    {
        public string Id { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public bool IsCancelled { get; set; }

        // Relacionamentos
        public string UserId { get; set; }
        public virtual UserQuery User { get; set; }

        public virtual List<SaleItemQuery> SaleItems { get; set; }
    }
}
