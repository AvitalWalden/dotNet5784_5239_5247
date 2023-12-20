namespace BlApi;

public interface IEngineer
{
    /// <summary>
    ///  creates new engineer
    /// </summary>
    /// <param name="item">the new task to be added</param>
    /// <returns></returns>
    public int Create(BO.Engineer item);
    /// <summary>
    /// returns engineer by given id
    /// </summary>
    /// <param name="id">id of engineer that should be returned</param>
    /// <returns></returns>
    public BO.Engineer? Read(int id);
    /// <summary>
    /// returns all the engineers that pass the condition
    /// </summary>
    /// <param name="filter">the condition that the engineer need</param>
    /// <returns>all the tasks that pass the condition</returns>
    public IEnumerable<BO.Engineer?> ReadAll(Func<BO.Engineer, bool>? filter = null);
    /// <summary>
    /// Updates engineer details
    /// </summary>
    /// <param name="item">the detailes of the engineer</param>
    public void Update(BO.Engineer item);
    /// <summary>
    /// Deletes a engineer by its Id
    /// </summary>
    /// <param name="id">id of the engineer that should be deleted</param>
    public void Delete(int id);
}
