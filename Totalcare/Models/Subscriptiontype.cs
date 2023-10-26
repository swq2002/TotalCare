using System;
using System.Collections.Generic;

namespace Totalcare.Models;

public partial class Subscriptiontype
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public string? Details { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
