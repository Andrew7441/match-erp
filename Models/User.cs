using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models;

public partial class User
{
    public int Id { get; set; }

    [Required]
    [EmailAddress] // value must look like an EmailAddress
    [MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = null!;
}
