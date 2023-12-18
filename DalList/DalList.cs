namespace Dal;
using DalApi;
sealed public class DalList : IDal
{
    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public void Reset()
    {
        Task.Reset();
        Dependency.Reset();
        Engineer.Reset();
    }
}
