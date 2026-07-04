using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class InventoryItem
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // starts as empty string instead of null

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

    }
}
