using BlApi;
namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task boTask)
    {
        if (boTask.Id < 0)
        {
            throw new Exception("Task ID must be a positive number");
        }
        if (string.IsNullOrWhiteSpace(boTask.Description))
        {
            throw new Exception("Task description cannot be empty or null");
        }
       
        DO.Task doTask = new DO.Task
        (
            boTask.Id,
            boTask.Alias,
            boTask.Description,
            boTask.CreatedAtDate,
            (TimeSpan)(boTask.StartDate - boTask.CompleteDate), //--
            false, //false / true
            boTask.StartDate,
            boTask.ScheduledStartDate,
            boTask.DeadlineDate,
            boTask.CompleteDate,
            boTask.Deliverables,
            boTask.Remarks,
            boTask.Engineer?.Id,
            (DO.EngineerExperience)boTask.ComplexityLevel //---
        ) ;
        foreach (BO.TaskInList doDependency in boTask.Dependencies)
        {
            DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
            int idDependency = _dal.Dependency.Create(doDepend);  //האם להחזיר את המזהה של התלויות שיצרנו
        }
        try
        {
            int idTask = _dal.Task.Create(doTask);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = doTask.CreatedAtDate,
            //Status = ,
            //Milestone = new BO.MilestoneInTask()
            //{
                
            //},
            //StartDate = doTask.StartDate,
            //ScheduledStartDate = doTask.ScheduledDate,
            //ForecastDate = doTask.ScheduledDate,
            //DeadlineDate = doTask.DeadlineDate,
            //CompleteDate = doTask.CompleteDate,
            //Deliverables = doTask.Deliverables,
            //Remarks = doTask.Remarks,
            //Engineer = new BO.EngineerInTask()
            //{
            //    Id = (int)(_dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId)?.Id!),
            //    Alias = _dal.Task.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId)?.Alias!
            //},
            //ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
        };
    }

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {

        IEnumerable<BO.Task> readAllTask = (from DO.Task doTask in _dal.Task.ReadAll()
                                                     select new BO.Task
                                                     {
                                                         Id = doTask.Id,
                                                         Description = doTask.Description,
                                                         Alias = doTask.Alias,
                                                         CreatedAtDate = doTask.CreatedAtDate,
                                                         //Status = ,
                                                         //Milestone = new BO.MilestoneInTask()
                                                         //{
                                                         //    Id = (int)(_dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doTask.Id)?.Id!),
                                                         //    Alias = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doTask.Id)?.Alias!
                                                         //},
                                                         //StartDate = doTask.StartDate,
                                                         //ScheduledStartDate = doTask.ScheduledDate,
                                                         //ForecastDate = doTask.,/////////////////
                                                         //DeadlineDate = doTask.DeadlineDate,
                                                         //CompleteDate = doTask.CompleteDate,
                                                         //Deliverables = doTask.Deliverables,
                                                         //Remarks = doTask.Remarks,
                                                         //Engineer = new BO.EngineerInTask()
                                                         //{
                                                         //    Id = (int)(_dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId)?.Id!),
                                                         //    Alias = _dal.Task.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId)?.Alias!
                                                         //},
                                                         //ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
                                                     });
        if (filter != null)
        {
            IEnumerable<BO.Task> readAllTaskFilter = from item in readAllTask
                                                           where filter(item)
                                                           select item;
            return readAllTaskFilter;
        }
        return readAllTask;
    }
    public void Update(BO.Task item)
    {
        throw new NotImplementedException();
    }
}
