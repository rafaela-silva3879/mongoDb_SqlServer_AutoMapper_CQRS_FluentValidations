using DeveloperStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Models.Queries
{
    public class UserQuery
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int UserProfile { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool  IsDeleted { get; set; }

        //For password reset
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetExpiresIn { get; set; }


        public virtual List<ProductQuery> Products { get; set; }
        public virtual List<SaleQuery> Sales { get; set; }
    }
}
