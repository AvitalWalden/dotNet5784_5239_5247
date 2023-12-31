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
        if (boTask.Alias == "")
        {
            throw new BO.BlInvalidValue("Task description cannot be empty or null");
        }
        TimeSpan? requiredEffort = null;
        if (boTask.StartDate != null || boTask.CompleteDate != null)
        {
            requiredEffort = (TimeSpan)(boTask.StartDate! - boTask.CompleteDate!);
        }
        DO.Task doTask = new DO.Task
        (
            boTask.Id,
            boTask.Description,
            boTask.Alias,
            boTask.CreatedAtDate,
            requiredEffort,
            false,
            boTask.StartDate,
            boTask.BaselineStartDate,
            boTask.DeadlineDate,
            boTask.CompleteDate,
            boTask.Deliverables,
            boTask.Remarks,
            boTask.Engineer?.Id ?? null,
            (DO.EngineerExperience)boTask.ComplexityLevel
        );
        if (boTask.Dependencies != null)
        {
            foreach (BO.TaskInList doDependency in boTask.Dependencies)
            {
                DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
                int idDependency = _dal.Dependency.Create(doDepend);
            }
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
    //public BO.Task? Read(int id)
    //{
    //    DO.Task? doTask = _dal.Task.Read(id);
    //    if (doTask == null)
    //        throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

    //    DO.Engineer? eng = _dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId);
    //    BO.EngineerInTask? engineer = null;
    //    if (eng != null)
    //    {
    //        engineer = new BO.EngineerInTask()
    //        {
    //            Id = eng.Id,
    //            Name = eng.Name
    //        };
    //    }


    //    List<BO.TaskInList>? dependencies = (List<BO.TaskInList>?)_dal.Dependency.ReadAll(depen => depen.DependsOnTask == doTask.Id).Select(doDependency =>
    //    {
    //        if (doDependency?.DependentTask != null)
    //        {
    //            DO.Task task = _dal.Task.Read((int)doDependency.DependentTask)!;
    //            return new BO.TaskInList()
    //            {
    //                Id = task.Id,
    //                Description = task.Description,
    //                Alias = task.Alias,
    //                Status = CalculateStatusOfTask(task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate)
    //            };
    //        }
    //        return null;
    //    }).Where(dependency => dependency != null);

    //    BO.TaskInList task = dependencies!.Where(t => _dal.Task.Read(t.Id)!.IsMilestone == true).FirstOrDefault()!;
    //    BO.MilestoneInTask milestone = new BO.MilestoneInTask()
    //    {
    //        Id = task.Id,
    //        Alias = task.Alias
    //    };
    //    return new BO.Task()
    //    {
    //        Id = doTask.Id,
    //        Alias = doTask.Alias,
    //        Description = doTask.Description,
    //        CreatedAtDate = doTask.CreatedAtDate,
    //        Status = CalculateStatusOfTask(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate),
    //        Dependencies = dependencies,
    //        Milestone = milestone,
    //        BaselineStartDate = doTask.ScheduledDate,
    //        //ScheduledStartDate = doTask.ScheduledDate,
    //        StartDate = doTask.StartDate,
    //        ForecastDate = doTask.StartDate + doTask.RequiredEffort,
    //        DeadlineDate = doTask.DeadlineDate,
    //        CompleteDate = doTask.CompleteDate,
    //        Deliverables = doTask.Deliverables,
    //        Remarks = doTask.Remarks,
    //        Engineer = engineer,
    //        ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
    //    };
    //}

    public BO.Task? Read(int id)
    {
         DO.Task? doTask = _dal.Task.Read(t => t.Id == id);
         if (doTask == null)
             throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

         List<BO.TaskInList>? tasksList=null;

         int milestoneId = _dal.Dependency.Read(d => d.DependentTask == doTask.Id)!.Id;
         DO.Task? milestoneAsATask = _dal.Task.Read(t => t.Id == milestoneId && t.IsMilestone);
         BO.MilestoneInTask? milestone = null;
         if (milestoneAsATask != null)
         {
             string aliasOfMilestone = milestoneAsATask.Alias;
             milestone = new BO.MilestoneInTask()
             {
                 Id = milestoneId,
                 Alias = aliasOfMilestone
             };
         }
         else
         {
             _dal.Dependency.ReadAll(d => d.DependentTask == doTask.Id)
                             .Select(d => _dal.Task.Read((int)d?.DependsOnTask!))
                             .ToList()
                             .ForEach(task =>
                             {
                                 tasksList?.Add(new BO.TaskInList()
                                 {
                                     Id = task!.Id,
                                     Alias = task.Alias,
                                     Description = task.Description,
                                     Status = CalculateStatusOfTask(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate)
                                 });
                             });
         }

         DO.Engineer? eng = _dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId);
         BO.EngineerInTask? engineer = null;
         if (eng != null)
         {
             engineer = new BO.EngineerInTask()
             {
                 Id = eng.Id,
                 Name = eng.Name
             };
         }

         return new BO.Task()
         {
             Id = doTask.Id,
             Alias = doTask.Alias,
             Description = doTask.Description,
             CreatedAtDate = doTask.CreatedAtDate,
             Status = CalculateStatusOfTask(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate),
             Dependencies = null,
             Milestone = milestone,
             BaselineStartDate = doTask.ScheduledDate,
             //ScheduledStartDate = doTask.ScheduledDate,
             StartDate = doTask.StartDate,
             ForecastDate = doTask.StartDate + doTask.RequiredEffort,
             DeadlineDate = doTask.DeadlineDate,
             CompleteDate = doTask.CompleteDate,
             Deliverables = doTask.Deliverables,
             Remarks = doTask.Remarks,
             Engineer = engineer,
             ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
         };
    }
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {

        IEnumerable<BO.Task?> readAllTask = _dal.Task.ReadAll().Select(doTask =>
        {
            if (doTask == null)
            {
                return null;
            }
            DO.Engineer? eng = _dal.Engineer.ReadAll().FirstOrDefault(engineer => engineer?.Id == doTask.EngineerId);
            BO.EngineerInTask? engineer = null;
            if (eng != null)
            {
                engineer = new BO.EngineerInTask()
                {
                    Id = eng.Id,
                    Name = eng.Name
                };
            }
            List<BO.TaskInList>? dependencies = (List<BO.TaskInList>?)_dal.Dependency.ReadAll(depen => depen.DependsOnTask == doTask.Id).Select(doDependency =>
            {
                if (doDependency?.DependentTask != null)
                {
                    DO.Task task = _dal.Task.Read((int)doDependency.DependentTask)!;
                    return new BO.TaskInList()
                    {
                        Id = task.Id,
                        Description = task.Description,
                        Alias = task.Alias,
                        Status = CalculateStatusOfTask(task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate)
                    };
                }
                return null;
            }).Where(dependency => dependency != null);

            BO.TaskInList task = dependencies!.Where(t => _dal.Task.Read(t.Id)!.IsMilestone == true).FirstOrDefault()!;
            BO.MilestoneInTask milestone = new BO.MilestoneInTask()
            {
                Id = task.Id,
                Alias = task.Alias
            };
            return new BO.Task
            {
                Id = doTask.Id,
                Alias = doTask.Alias,
                Description = doTask.Description,
                CreatedAtDate = doTask.CreatedAtDate,
                Status = CalculateStatusOfTask(doTask.StartDate, doTask.ScheduledDate, doTask.DeadlineDate, doTask.CompleteDate),
                Dependencies = dependencies,
                Milestone = milestone,
                BaselineStartDate = doTask.ScheduledDate,
                StartDate = doTask.StartDate,
                //ScheduledStartDate = doTask.ScheduledDate,
                ForecastDate = doTask.StartDate + doTask.RequiredEffort,
                DeadlineDate = doTask.DeadlineDate,
                CompleteDate = doTask.CompleteDate,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                Engineer =engineer,
                ComplexityLevel = (BO.EngineerExperience)doTask.ComplexityLevel,
            };

            
        }).Where(task => task != null).ToList(); // We will use WHERE to filter and drop the tasks that are NULL

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
        TimeSpan? requiredEffort = null;
        if (boTask.StartDate != null || boTask.CompleteDate != null)
        {
            requiredEffort = (TimeSpan)(boTask.StartDate! - boTask.CompleteDate!);
        }
        DO.Task doTask = new DO.Task
        (
            boTask.Id,
            boTask.Alias,
            boTask.Description,
            boTask.CreatedAtDate,
            requiredEffort,
            false,
            boTask.StartDate,
            boTask.BaselineStartDate,
            boTask.DeadlineDate,
            boTask.CompleteDate,
            boTask.Deliverables,
            boTask.Remarks,
            boTask.Engineer?.Id,
            (DO.EngineerExperience)boTask.ComplexityLevel
        );
        if (boTask.Dependencies != null)
        {
            foreach (BO.TaskInList doDependency in boTask.Dependencies)
            {
                DO.Dependency doDepend = new DO.Dependency(0, boTask.Id, doDependency.Id);
                int idDependency = _dal.Dependency.Create(doDepend);
            }
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
    public void Delete(int id)
    {
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message, ex);
        }
    }
    public static BO.Status CalculateStatusOfTask(DateTime? startDate, DateTime? ScheduledDate, DateTime? deadlineDate, DateTime? completeDate)
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
