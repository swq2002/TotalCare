using System;
using System.Collections.Generic;

namespace Totalcare.Models;

public partial class Bank
{
    public decimal Bankid { get; set; }

    public decimal? Cardnum { get; set; }

    public string? Expirydate { get; set; }

    public decimal? Cvv { get; set; }

    public decimal? Balance { get; set; }
}
