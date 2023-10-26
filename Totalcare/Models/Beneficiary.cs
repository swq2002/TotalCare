using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Totalcare.Models;

public partial class Beneficiary
{
    public decimal Beneficiaryid { get; set; }

    public decimal? Userid { get; set; }

    public string? Relationship { get; set; }

    public string? Proofdocument { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public string? Requeststatus { get; set; }

    public virtual Subscription? User { get; set; }


}
