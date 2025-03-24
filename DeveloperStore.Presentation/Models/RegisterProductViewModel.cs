using Microsoft.AspNetCore.Http;

namespace DeveloperStore.Presentation.Models
{
    public class RegisterProductViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string UnitPrice { get; set; }
        public IFormFile PhotoJPGJPEG { get; set; }
        public int Quantity { get; set; }
    }
}
