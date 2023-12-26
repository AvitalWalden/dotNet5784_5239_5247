using BlApi;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// The function receives a logical entity engineer checks the integrity of the data and creates a data entity engineer.
    /// </summary>
    /// <param name="boEngineer">a logical entity engineer</param>
    /// <returns>the id from the engineer</returns>
    /// <exception cref="BO.BlInvalidValue">checks the integrity of the data</exception>
    /// <exception cref="BO.BlAlreadyExistsException">this engineer akready exist</exception>
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
            BO.TaskInEngineer task = new BO.TaskInEngineer()
            {
                Id = (int)(_dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == engneerToDelete.Id)?.Id!),
                Alias = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == engneerToDelete.Id)?.Alias!
            };
            if(task?.Id != null)
            { 
            //    if(ITask.Read(task.Id)?.Status == BO.Status.OnTrack || ITask.Read(task.Id)?.Status == BO.Status.OnTrack)
            //    {

            //    }
            }
            _dal.Engineer.Delete(engneerToDelete.Id);//עשינו שאי אפשר לממחוק אז איך אפשר לשלוח לשם

        }
        else
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        }
    }

    /// <summary>
    ///  This method is used to read an Engineer by ID
    /// </summary>
    /// <param name="id">the name of the engineer who search for</param>
    /// <returns>Constructed engineer object</returns>
    /// <exception cref="BO.BlDoesNotExistException">No suitable data layer engineer exists</exception>
    public BO.Engineer? Read(int id) // לטפל אם אין לא משימה רוחמה או אפרת קליין
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
            Task = new BO.TaskInEngineer()
            {
                Id = (int)(_dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doEngineer.Id)?.Id),
                Alias = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doEngineer.Id)?.Alias!
            }
        };
    }

    /// <summary>
    /// returns all the engineers that pass the condition
    /// </summary>
    /// <param name="filter">the condition that the engineer need</param>
    /// <returns>all the tasks that pass the condition</returns>
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
                    Task = new BO.TaskInEngineer()
                    {
                        Id = (int)(_dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doEngineer.Id)?.Id!),
                        Alias = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doEngineer.Id)?.Alias!
                    }
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

    /// <summary>
    /// This method is used to update the engineer 
    /// </summary>
    /// <param name="boEngineer">The updated engineer</param>
    /// <exception cref="BO.BlInvalidValue">The entered value is incorrect</exception>
    /// <exception cref="BO.BlDoesNotExistException">$"Engineer does not exist"</exception>
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
