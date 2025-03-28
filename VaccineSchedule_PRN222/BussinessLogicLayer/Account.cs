using System;
using System.Collections.Generic;

namespace BussinessLogicLayer;

public partial class Account
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? LastUpdatedTime { get; set; }

    public virtual ICollection<ChildrenProfile> ChildrenProfiles { get; set; } = new List<ChildrenProfile>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
