namespace Dal;
using DalApi;
using System;

sealed public class DalList : IDal
{
    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public DateTime? startDateProject => DataSource.Config.startDateProject;

    public DateTime? endDateProject => DataSource.Config.endDateProject;

    public void Reset()
    {
        Task.Reset();
        Dependency.Reset();
        Engineer.Reset();
    }
}
