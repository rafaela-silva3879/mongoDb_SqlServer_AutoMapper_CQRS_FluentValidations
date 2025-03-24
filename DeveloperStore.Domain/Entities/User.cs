using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Entities
{
    public class User
    {
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public int UserProfile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Para resetar a senha
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpiresIn { get; set; }

        // Relacionamentos
        public virtual List<Product> Products { get; set; } // Produtos criados pelo usuário
        public virtual List<Sale> Sales { get; set; } // Vendas realizadas pelo usuário

    }
}
