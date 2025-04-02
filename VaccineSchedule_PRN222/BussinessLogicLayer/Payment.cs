using System;
using System.Collections.Generic;

namespace BussinessLogicLayer;

public partial class Payment
{
    public string Id { get; set; } = null!;

    public string ScheduleId { get; set; } = null!;

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? PaymentStatus { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;
}


