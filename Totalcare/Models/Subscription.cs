using System;
using System.Collections.Generic;

namespace Totalcare.Models;

public partial class Subscription
{
    public decimal Subscriptionid { get; set; }

    public decimal? Userid { get; set; }

    public DateTime? Subscriptiondate { get; set; }

    public string? Paymentstatus { get; set; }

    public decimal? Typeid { get; set; }

    public virtual ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();

    public virtual Subscriptiontype? Type { get; set; }

    public virtual User? User { get; set; }
}
