using System;
using System.Collections.Generic;

namespace ReverseEngineering.Models;

public partial class VaccineRecord
{
    public int VaccineRecordId { get; set; }

    public int UserId { get; set; }

    public int DependentId { get; set; }

    public int VaccineId { get; set; }

    public DateTime DateAdministered { get; set; }

    public string AdministeredLocation { get; set; } = null!;

    public string AdministeredBy { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Dependent Dependent { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Vaccine Vaccine { get; set; } = null!;
}
