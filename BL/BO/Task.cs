namespace BO;
/// <summary>
/// Creating an entity of Task and declaring the attributes
/// </summary>
public class Task
{
    public int Id { get; init; }
    public required string Alias { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAtDate { get; set; }
    public Status? Status { get; set; } 
    public List<TaskInList>? Dependencies { get; set; } 
    public MilestoneInTask? Milestone { get; set; } 
    public DateTime? BaselineStartDate { get; set; }
    public DateTime? ScheduledStartDate { get; set; } //the planned start date
    public DateTime? StartDate { get; set; } //the real start date
    public DateTime? ForecastDate { get; set; } 
    public DateTime? DeadlineDate { get; set; } //the latest complete date
    public DateTime? CompleteDate { get; set; } 
    public string? Deliverables { get; set; } 
    public string? Remarks { get; set; }
    // public Tuple<int, string>? Engineer { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience? ComplexityLevel { get; set; } = null;
    public override string ToString() => this.ToStringProperty();
}
