namespace BlApi;

public interface IBl
{

    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }

}