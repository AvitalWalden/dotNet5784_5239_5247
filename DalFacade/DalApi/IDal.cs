namespace DalApi;

public interface IDal
{
    ITask Task { get; }

    IDependency Dependency { get; }

    IEngineer Engineer { get; }

    DateTime? startDateProject { get; }

    DateTime? endDateProject { get; }

    public void Reset(); //Delete all the datasource
}