using System;
using System.Collections.Generic;

namespace BussinessLogicLayer;

public partial class ChildrenProfile
{
    public string Id { get; set; } = null!;

    public string ParentId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public virtual Account Parent { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
