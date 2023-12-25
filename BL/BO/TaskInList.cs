namespace BO;
/// <summary>
/// Creating an entity of TaskInList and declaring the attributes
/// </summary>
public class TaskInList
{
    public int Id { get; init; }
    public required string Description { get; set; }
    public required string Alias { get; set; }
    public Status? Status { get; set; } = null;
}