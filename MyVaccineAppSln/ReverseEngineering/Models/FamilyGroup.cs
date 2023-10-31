using System;
using System.Collections.Generic;

namespace ReverseEngineering.Models;

public partial class FamilyGroup
{
    public int FamilyGroupId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<User> UsersUsers { get; set; } = new List<User>();
}
