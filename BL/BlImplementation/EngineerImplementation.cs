using BlApi;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    
    public int Create(BO.Engineer boEngineer)
    {
        if(boEngineer.Id<=0)
        {
            throw new BO.BlInvalidValue("The ID value is invalid");
        }
        if (boEngineer.Cost <= 0)
        {
            throw new BO.BlInvalidValue("Incorrect price. The price must be positive");
        }
        if(boEngineer.Name!="")
        {
            throw new BO.BlInvalidValue("Invalid name");
        }
        string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        if (!System.Text.RegularExpressions.Regex.IsMatch(emailPattern, emailPattern))
        {
            throw new BO.BlInvalidValue("Invalid email");
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
            throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");
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
        };
    }

    public IEnumerable<BO.Engineer?> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        IEnumerable<BO.Engineer?> readAllEngineer = (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                    Email = doEngineer.Email,
                    Level = (BO.EngineerExperience)doEngineer.Level,
                    Cost = doEngineer.Cost,
                    Task = BO.TaskInEngineer(_dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doEngineer.Id)?.Id, _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doEngineer.Id)?.Alias)
                });
        if (filter != null)
        {
            IEnumerable<BO.Engineer> readAllEngineerFilter = from item in readAllEngineer
                                                       where filter(item)
                                                       select item;
            return readAllEngineerFilter;
        }
        return readAllEngineer;
    }

    public void Update(BO.Engineer boEngineer)
    {
        if (boEngineer.Id <= 0)
        {
            throw new BO.BlInvalidValue("The ID value is invalid");
        }
        if (boEngineer.Cost <= 0)
        {
            throw new BO.BlInvalidValue("Incorrect price. The price must be positive");
        }
        if (boEngineer.Name != "")
        {
            throw new BO.BlInvalidValue("Invalid name");
        }
        string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        if (!System.Text.RegularExpressions.Regex.IsMatch(emailPattern, emailPattern))
        {
            throw new BO.BlInvalidValue("Invalid email");
        }
        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Email, (DO.EngineerExperience)boEngineer.Level, boEngineer.Cost);
        try
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalDoesNotExistException)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={boEngineer.Id}  does not exist");
        }
    }
}
