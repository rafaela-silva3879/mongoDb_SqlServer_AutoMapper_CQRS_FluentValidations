using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Entities
{
    public class SaleItem
    {
        [BsonId]
        public string Id { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }

        // Relacionamentos
        public string ProductId { get; set; }
        public Product Product { get; set; } // Produto relacionado

        public string SaleId { get; set; }
        [BsonIgnore] // Ignora o relacionamento no MongoDB para evitar ciclos
        public Sale Sale { get; set; } // Referência à venda
    }
}
