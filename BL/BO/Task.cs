namespace BO;

public class Task
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public DateTime CreatedAtDate { get; set; }
    public TimeSpan RequiredEffort { get; set; }
    public bool IsMilestone { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? ScheduledDate { get; set; } = null;
    public DateTime? DeadlineDate { get; set; } = null;
    public DateTime? CompleteDate { get; set; } = null;
    public string? Deliverables { get; set; } = null;
    public string? Remarks { get; set; } = null;
    public int? EngineerId { get; set; } = null;
    public EngineerExperience ComplexityLevel { get; set; }
    public bool Active { get; set; }
    public override string ToString() => this.ToStringProperty();
}
