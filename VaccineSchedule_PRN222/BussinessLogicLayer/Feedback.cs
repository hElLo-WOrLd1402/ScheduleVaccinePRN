using System;
using System.Collections.Generic;

namespace BussinessLogicLayer;

public partial class Feedback
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string ScheduleId { get; set; } = null!;

    public int Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime? CreatedTime { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual Account User { get; set; } = null!;
}
