namespace BO;

public class EngineerInTask
{
    public int Id { get; init; }
    public required string Alias { get; set; }
    public override string ToString() => this.ToStringProperty();
}
