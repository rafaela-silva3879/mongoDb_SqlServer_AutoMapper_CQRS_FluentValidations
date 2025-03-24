using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using System;
using System.Collections.Generic;

namespace DeveloperStore.Presentation.Models
{
    public class SaleDataViewModel
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
