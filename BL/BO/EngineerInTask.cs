namespace BO;
/// <summary>
/// Creating an entity of EngineerInTask and declaring the attributes
/// </summary>
public class EngineerInTask
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
