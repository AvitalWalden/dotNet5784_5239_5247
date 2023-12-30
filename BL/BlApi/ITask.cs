namespace BlApi;

/// <summary>
/// functions that can be done on tasks.
/// </summary>
public interface ITask
{
    public int Create(BO.Task boTask);
    public BO.Task? Read(int id);
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task boTask);
    public void Delete(int id);
}
