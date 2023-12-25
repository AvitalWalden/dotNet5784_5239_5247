namespace DalApi;

public interface IDal
{
    ITask Task { get; }

    IDependency Dependency { get; }

    IEngineer Engineer { get; }

    DateTime? startDateProject { get; set; }

    DateTime? endDateProject { get; set; }

    public void Reset(); //Delete all the datasource
}