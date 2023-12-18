using DalApi;

namespace Dal;
//stage 3
sealed public class DalXml : IDal
{
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
