using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace demoapp.Models
{
    public class ItemModelView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Quantity  { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
    }

    public class ItemCreateModelView
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        
        public string? FileName { get; set; }
        
        public IFormFile UploadedFile { get; set; }

        public string? createdby { get; set; }
    }
}
