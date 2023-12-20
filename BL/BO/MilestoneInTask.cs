namespace BO;
/// <summary>
/// Creating an entity of MilestoneInTask and declaring the attributes
/// </summary>
public class MilestoneInTask
{
    public int Id { get; init; }
    public required string Alias { get; set; }
    public override string ToString() => this.ToStringProperty();
}
