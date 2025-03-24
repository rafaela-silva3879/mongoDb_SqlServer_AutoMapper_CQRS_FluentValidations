using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Queries
{
    public class ProductQuery
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Photo { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }
    }
}
