using System;
using System.Collections.Generic;

namespace Totalcare.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public decimal? Userid { get; set; }

    public string? Testimonialtext { get; set; }

    public DateTime? Testimonialdate { get; set; }

    public string? Testimonialsubject { get; set; }

    public string? Requeststatus { get; set; }

    public virtual User? User { get; set; }
}
