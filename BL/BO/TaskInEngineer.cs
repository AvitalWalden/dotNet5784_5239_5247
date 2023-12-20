namespace BO;
/// <summary>
/// Creating an entity of TaskInEngineer and declaring the attributes
/// </summary>
public class TaskInEngineer
{
    public int Id { get; init; }
    public required string Alias { get; set; }
    public override string ToString() => this.ToStringProperty();

}
