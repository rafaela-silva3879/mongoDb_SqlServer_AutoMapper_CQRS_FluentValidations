using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Entities
{
    public class Sale
    {
        [BsonId]
        public string Id { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public bool IsCancelled { get; set; }

        // Relacionamentos
        public string UserId { get; set; }
        public virtual User User { get; set; } // Usuário que comprou

        public virtual List<SaleItem> SaleItems { get; set; } // Lista de itens da venda
    }
}
