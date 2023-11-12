namespace DalApi;

public interface IDal
{
    ITask Task { get; }

    IDependency Dependency { get; }

    IEngineer Engineer { get; }

}
