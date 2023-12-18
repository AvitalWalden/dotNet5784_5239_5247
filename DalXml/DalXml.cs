using DalApi;
using System.Diagnostics;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();

    //private static readonly Lazy<DalXml> lazyInstance = new Lazy<DalXml>(() => new DalXml());
    //public static IDal Instance => lazyInstance.Value;

    //private static readonly object lockObject = new object();
    private DalXml() { }
    public ITask Task => new TaskImplementation();

    public IDependency Dependency =>  new DependencyImplementation();

    public IEngineer Engineer =>  new Engineerlementation();

    public DateTime? startDateProject => Config.startDateProject;

    public DateTime? endDateProject => Config.endDateProject;

    public void Reset()
    {
        Task.Reset();
        Dependency.Reset();
        Engineer.Reset();
    }
}
