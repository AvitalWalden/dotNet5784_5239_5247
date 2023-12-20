namespace BlApi;

public interface IMilestone
{
    public int Create(BO.Milestone item);
    public BO.Milestone? Read(int id);
    public void Update(BO.Milestone item);
}