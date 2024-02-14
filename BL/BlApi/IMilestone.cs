namespace BlApi;

public interface IMilestone
{
    /// <summary>
    /// creates new milestone
    /// </summary>
    /// <param name="item">the new milestone to be added</param>
    /// <returns></returns>
    public void Create();
    /// <summary>
    /// returns milestone by given id
    /// </summary>
    /// <param name="id">id of milestone that should be returned</param>
    /// <returns></returns>
    public BO.Milestone? Read(int id);
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null);
    /// <summary>
    /// Updates milestone details
    /// </summary>
    /// <param name="item">the detailes of the milestone</param>
    public void Update(BO.Milestone item);
}