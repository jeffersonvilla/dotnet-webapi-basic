using System;
using System.Collections.Generic;

namespace ReverseEngineering.Models;

public partial class User
{
    public int UserId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string AspNetUserId { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();

    public virtual AspNetUser AspNetUser { get; set; } = null!;

    public virtual ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();

    public virtual ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();

    public virtual ICollection<FamilyGroup> FamilyGroupsFamilyGroups { get; set; } = new List<FamilyGroup>();
}
