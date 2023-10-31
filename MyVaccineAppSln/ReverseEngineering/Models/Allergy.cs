using System;
using System.Collections.Generic;

namespace ReverseEngineering.Models;

public partial class Allergy
{
    public int AllergyId { get; set; }

    public string Name { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
