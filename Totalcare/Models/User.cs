using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Totalcare.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string? Password { get; set; }

    public string? Fullname { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public string? Profilepicture { get; set; }

    public decimal? Roleid { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
