using System;
using System.Collections.Generic;

namespace BussinessLogicLayer;

public partial class Vaccine
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string AgeGroup { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
