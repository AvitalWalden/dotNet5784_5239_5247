
using BlApi;

namespace BlImplementation;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public IMilestone Milestone => new MilestoneImplementation();
}
