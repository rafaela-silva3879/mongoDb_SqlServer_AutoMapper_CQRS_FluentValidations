using System;

namespace DeveloperStore.Presentation.Models
{
    public class GetAllProductsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Photo { get; set; }
        public string PhotoPath { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }

        //Relacionamento
        public int? IdUser { get; set; }

    }
}
