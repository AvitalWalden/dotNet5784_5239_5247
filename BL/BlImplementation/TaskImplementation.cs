using BlApi;
using BO;
using System.Xml.Linq;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task boTask)
    {
        if (boTask.Id < 0)
        {
            throw new BO.BlInvalidValue("Task ID must be a positive number");
        }
        if (string.IsNullOrWhiteSpace(boTask.Description))
        {
            throw new BO.BlInvalidValue("Task description cannot be empty or null");
        }
        DO.Task doTask = new DO.Task
        (
            boTask.Id,
            boTask.Description,
            boTask.Alias,
            boTask.CreatedAtDate,
            (TimeSpan)(boTask.StartDate - boTask.CompleteDate), //--
            false,//////////////////////////
            boTask.StartDate,
            boTask.ScheduledStartDate,
            boTask.DeadlineDate,
            boTask.CompleteDate,
            boTask.Deliverables,
            boTask.Remarks,
            boTask.Engineer?.Id ?? null,
            (DO.EngineerExperience)boTask.ComplexityLevel //---
        );
        foreach (BO.TaskInList doDependency in boTask.Dependencies)
        {
            DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
            _dal.Dependency.Create(doDepend);
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
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = CalculateStatusOfTask(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate),
            Dependencies = ((List<TaskInList>)(from DO.Dependency doDependency in _dal.Dependency.ReadAll()
                                               select new TaskInList()
                                               {
                                                   Id = (int)_dal.Dependency.ReadAll().FirstOrDefault(dependency => dependency?.DependsOnTask == doTask.Id)?.DependentTask!,
                                                   Description = _dal.Task.Read((int)_dal.Dependency.ReadAll().FirstOrDefault(dependency => dependency?.DependsOnTask == doTask.Id)?.DependentTask!).Description,
                                                   Alias = _dal.Task.Read((int)_dal.Dependency.ReadAll().FirstOrDefault(dependency => dependency?.DependsOnTask == doTask.Id)?.DependentTask!).Alias,
                                                   Status = CalculateStatusOfTask(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate)
                                               })
            ),
            Milestone = new BO.MilestoneInTask() /////????
            {
                Id = (int)(_dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doTask.Id)?.Id!),
                Alias = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doTask.Id)?.Alias!
            },
            BaselineStartDate = doTask.StartDate,//////////
            ScheduledStartDate = doTask.ScheduledDate,
            StartDate = doTask.StartDate,
            ForecastDate = doTask.DeadlineDate, //מה מחושב בו???
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = new EngineerInTask() { 
               Id = (int)(_dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId)?.Id!),
               Name = _dal.Engineer.Read((int)(_dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId)?.Id!))!.Name
            },
            ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
        };
    }

    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {

        IEnumerable<BO.Task?> readAllTask = _dal.Task.ReadAll().Select(doTask =>
        {
            if (doTask == null)
            {
                return null; // If the engineer is NULL, we will also return a NULL engineer
            }

            return new BO.Task
            {
                Id = doTask.Id,
                Description = doTask.Description,
                Alias = doTask.Alias,
                CreatedAtDate = doTask.CreatedAtDate,
                Status = CalculateStatusOfTask(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate), //??
                Milestone = new BO.MilestoneInTask() /////????
                {
                    Id = (int)(_dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doTask.Id)?.Id!),
                    Alias = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doTask.Id)?.Alias!
                },
                Dependencies = ((List<TaskInList>)(from DO.Dependency doDependency in _dal.Dependency.ReadAll()
                                                   select new TaskInList()
                                                   {
                                                       Id = (int)_dal.Dependency.ReadAll().FirstOrDefault(dependency => dependency?.DependsOnTask == doTask.Id)?.DependentTask!,
                                                       Description = _dal.Task.Read((int)_dal.Dependency.ReadAll().FirstOrDefault(dependency => dependency?.DependsOnTask == doTask.Id)?.DependentTask!).Description,
                                                       Alias = _dal.Task.Read((int)_dal.Dependency.ReadAll().FirstOrDefault(dependency => dependency?.DependsOnTask == doTask.Id)?.DependentTask!).Alias,
                                                       //  Status = BO.Status.Scheduled   //???
                                                   })
                                                              ),
                StartDate = doTask.StartDate,
                ScheduledStartDate = doTask.ScheduledDate,
                ForecastDate = doTask.DeadlineDate, //מה מחושב בו???
                DeadlineDate = doTask.DeadlineDate,
                CompleteDate = doTask.CompleteDate,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                Engineer = new EngineerInTask()
                {
                    Id = (int)(_dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId)?.Id!),
                    Name = _dal.Engineer.Read((int)(_dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId)?.Id!))!.Name
                },
                ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
            };

            
        }).Where(task => task != null); // We will use WHERE to filter and drop the tasks that are NULL

        if (filter != null)
        {
            IEnumerable<BO.Task> readAllTaskFilter = from item in readAllTask
                                                     where filter(item)
                                                     select item;
            return readAllTaskFilter;
        }
        return readAllTask;
    }
    public void Update(BO.Task boTask)
    {
        if (boTask.Id < 0)
        {
            throw new Exception("Task ID must be a positive number");
        }
        if (string.IsNullOrWhiteSpace(boTask.Description))
        {
            throw new Exception("Task description cannot be empty or null");
        }
        //if (boTask.StartDate == null || boTask.CompleteDate == null)
        //{
        //    TimeSpan requiredEffort = 
        //}
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
        );
        foreach (BO.TaskInList doDependency in boTask.Dependencies)
        {
            DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
            int idDependency = _dal.Dependency.Create(doDepend);  //האם להחזיר את המזהה של התלויות שיצרנו
        }
        try
        {
            _dal.Task.Update(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }
    public BO.Status CalculateStatusOfTask(DateTime? startDate, DateTime? ScheduledDate, DateTime? deadlineDate, DateTime? completeDate)
    {
        if (startDate == null && deadlineDate == null)
            return BO.Status.Unscheduled;

        if (startDate != null && deadlineDate != null && completeDate == null)
            return BO.Status.Scheduled;

        if (startDate != null && completeDate != null && completeDate <= ScheduledDate)
            return BO.Status.OnTrack;

        if (startDate != null && completeDate != null && completeDate > ScheduledDate)
            return BO.Status.InJeopardy;

        return BO.Status.Unscheduled;
    }
}
