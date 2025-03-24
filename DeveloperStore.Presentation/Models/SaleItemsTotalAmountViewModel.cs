using System.Collections.Generic;

namespace DeveloperStore.Presentation.Models
{
    public class SaleItemsTotalAmountViewModel
    {
        public List<SaleItemsViewModel> SaleItemsList { get; set; }
        public decimal PurchaseTotalAmount { get; set; }    
    }
}
