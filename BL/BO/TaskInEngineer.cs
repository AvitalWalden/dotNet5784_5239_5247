namespace BO;

public class TaskInEngineer
{
    public int Id { get; init; }
    public required string Alias { get; set; }
    public override string ToString() => this.ToStringProperty();

}
