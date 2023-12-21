using BlApi;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer boEngineer)
    {
        if(boEngineer.Id<=0)
        {
            throw new ArgumentException("");
        }
        if (boEngineer.Cost <= 0)
        {
            throw new ArgumentException("");
        }
        if(boEngineer.Name!="")
        {
            throw new ArgumentException("");
        }
        if (boEngineer.Email != "")
        {
            throw new ArgumentException("");
        }
        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Email, (DO.EngineerExperience)boEngineer.Level, boEngineer.Cost);
        try
        {
            int idStud = _dal.Engineer.Create(doEngineer);
            return idStud;
        }
        catch /*(DO.DalAlreadyExistsException ex)*/
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists");
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Engineer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.Engineer?> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Engineer item)
    {
        throw new NotImplementedException();
    }
}
