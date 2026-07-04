// Using Data Entity Model which means create a C# model, connect it to a DB and bind it to the Razor Table
// Entity is just a C# Class that represents a DB table
//
using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Staff
    {
        [Key]
        public int ID { get; set; } // Primary Key

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // Field 1 

        [Required]
        [MaxLength(2)]
        public string Age { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty; // Field 2

        public decimal income { get; set; }

    }
}