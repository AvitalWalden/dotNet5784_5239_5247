using BlApi;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer engineer)
    {
          
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            //Task = (TaskInEngineer)doEngineer.Task,
        };
    }

    public IEnumerable<BO.Engineer?> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                    Email = doEngineer.Email,
                    Level = (BO.EngineerExperience)doEngineer.Level,
                    Cost = doEngineer.Cost,
                    //Task = (TaskInEngineer)doEngineer.Task,
                });
    }

    public void Update(BO.Engineer item)
    {
        throw new NotImplementedException();
    }

    private void ValidateEngineerData(BO.Engineer engineer)
    {
        if (engineer.Id <= 0)
            throw new ArgumentException("Engineer ID must be a positive number");

        if (string.IsNullOrWhiteSpace(engineer.Name))
            throw new ArgumentException("Engineer name cannot be empty or null");

        if (engineer.Cost <= 0)
            throw new ArgumentException("Engineer cost must be a positive number");

        if (string.IsNullOrWhiteSpace(engineer.Email))
            throw new ArgumentException("Engineer email cannot be empty or null");
    }
}
