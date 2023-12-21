using BlApi;
using BO;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    
    public int Create(BO.Engineer boEngineer)
    {
        if(boEngineer.Id<=0)
        {
            throw new BlInvalidValue("The ID value is invalid");
        }
        if (boEngineer.Cost <= 0)
        {
            throw new BlInvalidValue("Incorrect price. The price must be positive");
        }
        if(boEngineer.Name!="")
        {
            throw new BlInvalidValue("Invalid name");
        }
        string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        if (!System.Text.RegularExpressions.Regex.IsMatch(emailPattern, emailPattern))
        {
            throw new BlInvalidValue("Invalid email");
        }
        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Email, (DO.EngineerExperience)boEngineer.Level, boEngineer.Cost);
        try
        {
            int idStud = _dal.Engineer.Create(doEngineer);
            return idStud;
        }
        catch (DO.DalAlreadyExistsException)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists");
        }
    }

    public void Delete(int id)
    {
        //if (_dal.Task.ReadA(task => task?.EngineerId == id);
            //throw new DalDeletionImpossible($"Engineer with ID={id} cannot be deleted");
        DO.Engineer? engneerToDelete = _dal.Engineer.Read(id);
        if (engneerToDelete is not null)
        {
            _dal.Engineer.Delete(engneerToDelete.Id);//עשינו שאי אפשר לממחוק אז איך אפשר לשלוח לשם

        }
        else
        {
            //throw new BlDoesNotExistException($"Engineer with ID={id} not exists");
        }
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
}
