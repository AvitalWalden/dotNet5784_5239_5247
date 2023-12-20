namespace BO;
public class Engineer
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double Cost { get; set; }
    public bool Active { get; set; } = true;
    public TaskInEngineer? Task { get; set; }
    public override string ToString() => this.ToStringProperty();
}
