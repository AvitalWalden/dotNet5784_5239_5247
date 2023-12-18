using DalApi;
namespace Dal;
//stage 3
sealed public class DalXml : IDal
{
    public ITask Task => new TaskImplementation();

    public IDependency Dependency =>  new DependencyImplementation();

    public IEngineer Engineer =>  new Engineerlementation();

    public void Reset()
    {
        Task.Reset();
        Dependency.Reset();
        Engineer.Reset();
    }
}
