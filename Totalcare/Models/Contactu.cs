using System;
using System.Collections.Generic;

namespace Totalcare.Models;

public partial class Contactu
{
    public decimal Id { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Phonenumber { get; set; }

    public string? Message { get; set; }
}
