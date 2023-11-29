namespace DO;
/// <summary>
/// Dependency entity represents a task with all its props
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number)</param>
/// <param name="Description">Description of the task</param>
/// <param name="Alias">nickname of the task</param>
/// <param name="Milestone">Milestone (Boolean)</param>
/// <param name="Start">Creation date of the task</param>
/// <param name="ForecastDate">Estimated date of completion the task</param>
/// <param name="Deadline">Final date for completion the task</param>
/// <param name="Complete">Task completion date</param>
/// <param name="Deliverables">Product (a string describing the product)</param>
/// <param name="Remarks">Notes of the task</param>
/// <param name="EngineerId">The ID of the engineer assigned to the task</param>
/// <param name="ComplexityLevel">Difficulty level of the task</param>
public record Task
(
    int Id,
    string Description,
    string Alias,
    bool Milestone = false,
    DateTime? Start = null,
    DateTime? ForecastDate = null,
    DateTime? Deadline = null,
    DateTime? Complete = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null,
    EngineerExperience? ComplexityLevel = null,
    bool Active = true
)
{
    public Task() : this(0, "", "") { } //empty ctor for stage 3

    /// <summary>
    /// CreatedAt - create date of the current task record
    /// </summary>
    public DateTime CreatedAt => DateTime.Now; //get only
}
