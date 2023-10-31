using System;
using System.Collections.Generic;

namespace ReverseEngineering.Models;

public partial class Vaccine
{
    public int VaccineId { get; set; }

    public string Name { get; set; } = null!;

    public bool RequiresBooster { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();

    public virtual ICollection<VaccineCategory> CategoriesVaccineCategories { get; set; } = new List<VaccineCategory>();
}
