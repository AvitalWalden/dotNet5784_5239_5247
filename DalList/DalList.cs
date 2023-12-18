namespace Dal;
using DalApi;
using System;

sealed internal class DalList : IDal
{
    //public static IDal Instance { get; } = new DalList();

    //private static readonly Lazy<DalList> lazyInstance = new Lazy<DalList>(() => new DalList());
    //public static IDal Instance => lazyInstance.Value;

    //private static readonly object lockObject = new object();
    private DalList() { }

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
