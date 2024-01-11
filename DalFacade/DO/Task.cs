namespace DO;
/// <summary>
/// Dependency entity represents a task with all its props
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number)</param>
/// <param name="Description">Description of the task</param>
/// <param name="Alias">nickname of the task</param>
/// <param name="CreatedAtDate">Creation date of the task</param>
/// <param name="RequiredEffort">required effort time to the tast</param>
/// <param name="IsMilestone">Milestone (Boolean)</param>
/// <param name="StartDate">start date of the task</param>
/// <param name="ScheduledDate">Estimated date of completion the task</param>
/// <param name="DeadlineDate">Final date for completion the task</param>
/// <param name="CompleteDate">Task completion date</param>
/// <param name="Deliverables">Product (a string describing the product)</param>
/// <param name="Remarks">Notes of the task</param>
/// <param name="EngineerId">The ID of the engineer assigned to the task</param>
/// <param name="ComplexityLevel">Difficulty level of the task</param>
/// <param name="Active">The task is active or not</param>
public record Task
(
    int Id,
    string Description,
    string Alias,
    DateTime CreatedAtDate,
    TimeSpan? RequiredEffort = null,
    bool IsMilestone = false,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null,
    EngineerExperience ComplexityLevel = EngineerExperience.None,
    bool Active = true
)
{
    public Task() : this(0, "", "", DateTime.Now, TimeSpan.Zero) { } //empty ctor for stage 3
}