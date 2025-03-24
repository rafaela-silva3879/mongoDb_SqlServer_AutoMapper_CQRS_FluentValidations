using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Entities
{
    public class Product
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Photo { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Relacionamento
        public string UserId { get; set; } // ID do usuário que criou o produto

        public virtual User User { get; set; } // Usuário que cadastrou o produto
    }
}
