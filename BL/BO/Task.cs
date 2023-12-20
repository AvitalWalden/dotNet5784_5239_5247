﻿namespace BO;
using DO;
public class Task
{
    public int Id { get; set; }
    public required string Description { get; set; } // דרש
    public required string Alias { get; set; } // דרש
    public required DateTime CreatedAtDate { get; set; } //required?
    public Status? Status { get; set; } = null;
    public MilestoneInTask? Milestone { get; set; } = null;
    public DateTime? BaselineStartDate { get; set; } = null;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? ScheduledStartDate { get; set; } = null;
    public DateTime? ForecastDate { get; set; } = null;
    public DateTime? DeadlineDate { get; set; } = null;
    public DateTime? CompleteDate { get; set; } = null;
    public string? Deliverables { get; set; } = null;
    public string? Remarks { get; set; } = null;
    public EngineerInTask? Engineer { get; set; } = null;
    public EngineerExperience ComplexityLevel { get; set; }
    public override string ToString() => this.ToStringProperty();
}
