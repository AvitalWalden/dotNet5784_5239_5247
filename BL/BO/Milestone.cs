namespace BO;
/// <summary>
/// Creating an entity of Milestone and declaring the attributes
/// </summary>
public class Milestone
{
    public int Id { get; init; }
    public required string Description { get; set; }
    public required string Alias { get; set; }
    public DateTime CreatedAtDate { get; set; }
    public Status? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public double? CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<TaskInList>? Dependencies { get; set; }
    public override string ToString() => this.ToStringProperty();
}
