using Microsoft.AspNetCore.Http;
using System;

namespace DeveloperStore.Presentation.Models
{
    public class EditProductViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Quantity { get; set; }
        public string Name { get; set; }
        public string UnitPrice { get; set; }
        public string Photo { get; set; }
        public IFormFile PhotoJPGJPEG { get; set; }
    }
}
