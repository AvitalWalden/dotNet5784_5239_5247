namespace BO;
/// <summary>
/// Creating an entity of Milestone and declaring the attributes
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public required string Description { get; set; } // דרש
    public required string Alias { get; set; } // דרש
    public required DateTime CreatedAtDate { get; set; } //required?
    public Status? Status { get; set; } = null;
    public DateTime? StartDate { get; set; } = null;
    public DateTime? ForecastDate { get; set; } = null;
    public DateTime? DeadlineDate { get; set; } = null;
    public DateTime? CompleteDate { get; set; } = null;
    public double? CompletionPercentage { get; set; } = null;
    public string? Remarks { get; set; } = null;
    public List<TaskInList>? Dependencies { get; set; } = null;
    public override string ToString() => this.ToStringProperty();
}
