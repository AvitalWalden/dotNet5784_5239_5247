using BlApi;
using BO;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Algorithm for calculating milestones
    /// </summary>
    /// <exception cref="BO.BlFailedToCreateMilestone"></exception>
    public void Create()
    {
        
        var dependenciesForDistinct = _dal.Dependency.ReadAll()
            .OrderBy(dep => dep?.DependsOnTask)
            .GroupBy(dep => dep?.DependentTask, dep => dep?.DependsOnTask,
            (id, dependency) => new { TaskId = id, Dependencies = dependency })
            .ToList();

        var distinctDependencies = dependenciesForDistinct;
        for (var j = 0; j < dependenciesForDistinct.Count(); j++)//מחיקת כפלויות
        {
            var distinct = from d in dependenciesForDistinct
                           where d.TaskId != dependenciesForDistinct[j].TaskId && d.Dependencies.SequenceEqual(dependenciesForDistinct[j].Dependencies)
                        select d.TaskId;
            if (distinct.Count() >= 1)
            {
                distinctDependencies.Remove(distinctDependencies[j]);
            }
        }

        var groupedDependencies = _dal.Dependency.ReadAll()
          .OrderBy(dep => dep?.DependsOnTask)
          .GroupBy(dep => dep?.DependentTask, dep => dep?.DependsOnTask,
          (id, dependency) => new { TaskId = id, Dependencies = dependency })
          .ToList();

        int index = 1;
        List<DO.Dependency> dependencies = new List<DO.Dependency>();

        foreach (var dep in distinctDependencies)
        {

            DO.Task doTaskOfMilestone =
            new DO.Task
            {
                Id = 0, // השמה של מזהה המשימה התלויה
                Description = $"Milestone for Task {index}", // תיאור אוטומטי
                Alias = $"M{index}", // קיצור אוטומטי
                CreatedAtDate = DateTime.Now, // זמן יצירה
                RequiredEffort = null,
                IsMilestone = true
            };


            try
            {
                int milestoneId = _dal.Task.Create(doTaskOfMilestone);

                foreach (var idTask in dep.Dependencies)
                {
                    dependencies.Add(new DO.Dependency
                    {
                        Id = 0,
                        DependentTask = milestoneId,
                        DependsOnTask = idTask!.Value
                    });
                }

                foreach (var dependencyGroup in groupedDependencies)
                {
                    if (dependencyGroup.Dependencies.SequenceEqual(dep.Dependencies))
                    {
                        dependencies.Add(new DO.Dependency
                        {
                            DependentTask = dependencyGroup.TaskId!.Value,
                            DependsOnTask = milestoneId
                        });
                    }
                }
               
                index++;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlFailedToCreateMilestone($"failed to create Milestone with Alias = M{index}", ex);
            }

        }


        var independentOnTasks = _dal.Task.ReadAll()
          .Where(task => !dependencies.Any(d => d.DependentTask == task!.Id))
          .Select(task => task!.Id)
          .ToList();

        DO.Task projectStartMilestone = new DO.Task
        {
            Id = 0, // השמה של מזהה המשימה התלויה
            Description = $"start milestone", // תיאור אוטומטי
            Alias = $"Start", // קיצור אוטומטי
            CreatedAtDate = DateTime.Now,
            RequiredEffort = null,
            IsMilestone = true
        };

        //משימות ששום משימה לא תלויה בהן
        var independentTasks = _dal.Task.ReadAll()
         .Where(task => !dependencies.Any(d => d.DependsOnTask == task!.Id))
         .Select(task => task!.Id)
         .ToList();

        DO.Task projectEneMilestone = new DO.Task
        {
            Id = 0, // השמה של מזהה המשימה התלויה
            Description = $"end milestone", // תיאור אוטומטי
            Alias = $"End", // קיצור אוטומטי
            CreatedAtDate = DateTime.Now, // זמן יצירה
            RequiredEffort = null,
            IsMilestone = true
        };

        try
        {
            int startMilestoneId = _dal.Task.Create(projectStartMilestone);
            int endMilestoneId = _dal.Task.Create(projectEneMilestone);

            foreach (var task in independentOnTasks)
            {
                dependencies.Add(new DO.Dependency
                {
                    DependentTask = task,
                    DependsOnTask = startMilestoneId
                });
            }

            foreach (var task in independentTasks)
            {
                dependencies.Add(new DO.Dependency
                {
                    DependentTask = endMilestoneId,
                    DependsOnTask = task
                });
            }
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlFailedToCreateMilestone("Failed to create END or START milestone", ex);
        }

        _dal.Dependency.ReadAll().ToList().ForEach(d => _dal.Dependency.Delete(d!.Id));
        dependencies.ToList().ForEach(d => _dal.Dependency.Create(d));

        var milstoneList = _dal.Task.ReadAll(task => task.IsMilestone).Where(t => t != null).ToList();
        DateTime? scheduledDate;
        foreach (var milstone in milstoneList)
        {
            if (milstone!.Alias == "Start")
            {
                scheduledDate = DateTime.Now;
            }
            else
            {
                //scheduledDate = _dal.Task.ReadAll(task => _dal.Dependency.ReadAll().Any(dependency => dependency?.DependentTask == task.Id && dependency.DependsOnTask == milstone.Id)).Min(t => t!.ScheduledDate!);
                scheduledDate = _dal.Task.ReadAll(task => _dal.Dependency.ReadAll().Any(dependency => dependency?.DependentTask == milstone.Id && dependency.DependsOnTask == task.Id)).Min(t => t!.ScheduledDate!);

            }
        }
        DateTime? DeadlineDate;
        foreach (var milstone in milstoneList)
        {
            if (milstone!.Alias == "End")
            {
                DeadlineDate = DalApi.Factory.Get.endDateProject;
            }
            else
            {
                //DeadlineDate = _dal.Task.ReadAll(task => _dal.Dependency.ReadAll().Any(dependency => dependency?.DependentTask == task.Id && dependency.DependsOnTask == milstone.Id)).Max(t => t!.DeadlineDate);
                DeadlineDate = _dal.Task.ReadAll(task => _dal.Dependency.ReadAll().Any(dependency => dependency?.DependentTask == milstone.Id && dependency.DependsOnTask == task.Id)).Max(t => t!.DeadlineDate);

            }
        }

    }

    /// <summary>
    /// Milestone details request 
    /// You will receive a Milestone ID You will attempt to request a milestone(an entity of type task) from a data layer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.FailedToReadMilestone"></exception>
    public BO.Milestone? Read(int id)
    {
        try
        {
            DO.Task? doTaskMilestone = _dal.Task.Read(task => task.Id == id && task.IsMilestone);
            if (doTaskMilestone == null)
                throw new BO.BlDoesNotExistException($"Milstone with ID={id} does Not exist");

            var idMilstoneDependsOn = _dal.Dependency.ReadAll(dependensy => dependensy.DependentTask == doTaskMilestone.Id)
                                        .Select(dependensy => dependensy?.DependsOnTask);
            var milstoneDependsOn = _dal.Task.ReadAll(task => idMilstoneDependsOn.Contains(task.Id)).ToList();

            var tasksInList = milstoneDependsOn.Select(task => new BO.TaskInList
            {
                Id = task!.Id,
                Description = task.Description,
                Alias = task.Alias,
                Status = Tools.CalculateStatusOfTask(task)
            }).ToList();

            return new BO.Milestone()
            {
                Id = doTaskMilestone.Id,
                Description = doTaskMilestone.Description,
                Alias = doTaskMilestone.Alias,
                CreatedAtDate = doTaskMilestone.CreatedAtDate,
                Status = Tools.CalculateStatusOfTask(doTaskMilestone),
                ForecastDate = doTaskMilestone.ScheduledDate,
                DeadlineDate = doTaskMilestone.DeadlineDate,
                CompleteDate = doTaskMilestone.CompleteDate,
                CompletionPercentage = (tasksInList.Count(task => task.Status == BO.Status.OnTrack) / (double)tasksInList.Count) * 100,
                Remarks = doTaskMilestone.Remarks,
                Dependencies = tasksInList
            };
        }
        catch (BO.FailedToReadMilestone)
        {
            throw new BO.FailedToReadMilestone($"Milestone with ID={id} cannwt be read");
        }
    }

    /// <summary>
    ///  Updates milestone details
    /// </summary>
    /// <param name="boMilestone"></param>
    /// <exception cref="Exception"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Milestone boMilestone)
    {
        if (string.IsNullOrWhiteSpace(boMilestone.Alias))
        {
            throw new Exception("Milestone alias cannot be empty or null");
        }
        if (string.IsNullOrWhiteSpace(boMilestone.Description))
        {
            throw new Exception("Milestone description cannot be empty or null");
        }
        if (string.IsNullOrWhiteSpace(boMilestone.Remarks))
        {
            throw new Exception("Milestone remarks cannot be empty or null");
        }
        DO.Task? task = _dal.Task.Read(boMilestone.Id);
        if (task == null)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boMilestone.Id} does not exist");
        }
        else
        {
           DO.Task doMilestone = new DO.Task
           (
               boMilestone.Id,
               boMilestone.Description,
               boMilestone.Alias,
               task.CreatedAtDate,
               task.RequiredEffort,
               task.IsMilestone,
               task.StartDate,
               task.ScheduledDate,
               task.DeadlineDate,
               task.CompleteDate,
               task.Deliverables,
               boMilestone.Remarks,
               task.EngineerId,
               task.ComplexityLevel
           );
           try
           {
               _dal.Task.Update(doMilestone);
           }
           catch (DO.DalDoesNotExistException)
           {
               throw new BO.BlDoesNotExistException($"Task with ID={boMilestone.Id} does not exist");
           }
        }
       
    }

    private void SetDeadLineDateForTask()
    {
        //Stop condition
        //if (idOfTask == idOfStartMilestone)
        //    return;
        ////The data of current checked task
        //DO.Task? dependentTask = _dal.Task.Read(idOfTask ?? throw new BO.BlNullPropertyException("id Of Task can't be null"));

        //var DependsOnTaskList = dependenciesList.Where(dep => dep?.DependentTask == idOfTask)
        //    .Select(dep => dep?.DependsOnTask).ToList();
        //foreach (int? taskId in DependsOnTaskList)
        //{
        //    DO.Task currentTask = _dal.Task.Read(taskId) ?? throw new BO.BlNullPropertyException("id Of Task can't be null");
        //    DateTime? deadlineTime = dependentTask.LastEndDate - dependentTask.RequiredEffortTime;
        //    //If there is a milestone that depends on 2 tasks, determining its start time according to the longer time of the 2 tasks
        //    if (currentTask.Milestone == true && (currentTask.LastEndDate > deadlineTime || currentTask is null))
        //    {
        //        _dal.Task.Update(new DO.Task(currentTask.IdNumberTask, currentTask.Alias, currentTask.Description, currentTask.CreatedAtDate, currentTask.RequiredEffortTime, currentTask.Milestone, currentTask.Product, currentTask.Notes, currentTask.Level, currentTask.idEngineer, currentTask.StartDate, currentTask.scheduleDate, deadlineTime, null));

        //    }
        //    else
        //        _dal.Task.Update(new DO.Task(currentTask.IdNumberTask, currentTask.Alias, currentTask.Description, currentTask.CreatedAtDate, currentTask.RequiredEffortTime, currentTask.Milestone, currentTask.Product, currentTask.Notes, currentTask.Level, currentTask.idEngineer, currentTask.StartDate, currentTask.scheduleDate, deadlineTime, null));
        //    SetDeadLineDateForTask(taskId, idOfStartMilestone, dependenciesList);//A call in recursion to each of the tasks in the list to calculate its completion time according to the algorithm
        //}
    }
}