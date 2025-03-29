using System;
using System.Collections.Generic;

namespace BussinessLogicLayer;

public partial class Schedule
{
    public string Id { get; set; } = null!;

    public string ChildId { get; set; } = null!;

    public string VaccineId { get; set; } = null!;

    public DateTime AppointmentDate { get; set; }

    public string? Status { get; set; }

    public virtual ChildrenProfile Child { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Vaccine Vaccine { get; set; } = null!;
}
