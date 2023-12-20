namespace BlApi;

/// <summary>
/// functions that can be done on tasks.
/// </summary>
public interface ITask
{
    public int Create(BO.Task item);
    public BO.Task? Read(int id);
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task item);
    public void Delete(int id);
}
