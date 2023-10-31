using System;
using System.Collections.Generic;

namespace ReverseEngineering.Models;

public partial class Dependent
{
    public int DependentId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();
}
