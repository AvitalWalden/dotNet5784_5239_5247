using BlApi;
using System.Threading.Tasks;

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
        for (var j = 0; j < dependenciesForDistinct.Count(); j++)
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
                Id = 0, 
                Description = $"Milestone for Task {index}",
                Alias = $"M{index}", 
                CreatedAtDate = DateTime.Now, 
                RequiredEffort = TimeSpan.Zero,
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
            Id = 0,
            Description = $"start milestone",
            Alias = $"Start",
            CreatedAtDate = DateTime.Now,
            RequiredEffort = TimeSpan.Zero,
            IsMilestone = true,
            ScheduledDate = DateTime.Now,

        };

        //משימות ששום משימה לא תלויה בהן
        var independentTasks = _dal.Task.ReadAll()
         .Where(task => !dependencies.Any(d => d.DependsOnTask == task!.Id))
         .Select(task => task!.Id)
         .ToList();

        DO.Task projectEndMilestone = new DO.Task
        {
            Id = 0,
            Description = $"end milestone",
            Alias = $"End",
            CreatedAtDate = DateTime.Now,
            RequiredEffort = TimeSpan.Zero,
            IsMilestone = true,
            DeadlineDate = DalApi.Factory.Get.endDateProject


        };

        try
        {
            int startMilestoneId = _dal.Task.Create(projectStartMilestone);
            int endMilestoneId = _dal.Task.Create(projectEndMilestone);

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

        DO.Task endTask = _dal.Task.Read(task => task.Alias == "End")!;
        DO.Task startTask = _dal.Task.Read(task => task.Alias == "Start")!;

        SetDeadLineDateForTask(endTask, startTask);

        DO.Task endTask1 = _dal.Task.Read(task => task.Alias == "End")!;
        DO.Task startTask1 = _dal.Task.Read(task => task.Alias == "Start")!;

        SetScheduledDateForTask(startTask1, endTask1);
        setNameOfMilestone();
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
                Status = BO.Tools.CalculateStatusOfTask(task)
            }).ToList();

            return new BO.Milestone()
            {
                Id = doTaskMilestone.Id,
                Description = doTaskMilestone.Description,
                Alias = doTaskMilestone.Alias,
                CreatedAtDate = doTaskMilestone.CreatedAtDate,
                Status = BO.Tools.CalculateStatusOfTask(doTaskMilestone),
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

    /// <summary>
    /// A function that calculates for each task its start date according to calculations of all the dates in the project
    /// </summary>
    private void SetScheduledDateForTask(DO.Task startTask, DO.Task endTask)
    {
        if (startTask.Id == endTask.Id)
            return;

        var allDependencies = _dal.Dependency
           .ReadAll()
           .Where(dependency => dependency != null);

        var DependentTaskList = allDependencies.Where(dep => dep?.DependsOnTask == startTask.Id)
          .Select(dep => dep?.DependentTask).ToList();

        foreach (int? taskId in DependentTaskList)
        {
            DO.Task? currentTask = _dal.Task.Read(taskId ?? throw new BO.BlNullPropertyException("id Of Task can't be null"));
            DateTime? ScheduleTime = startTask.ScheduledDate + startTask.RequiredEffort;
            if (startTask.DeadlineDate + currentTask!.RequiredEffort > currentTask.DeadlineDate)
                throw new BO.BlPlanningOfProjectTimesException($"According to the date restrictions, the task {taskId} does not have time to be completed in its entirety");

            if (currentTask.ScheduledDate == null || currentTask.ScheduledDate < ScheduleTime)
            {
                _dal.Task.Update(
                 new DO.Task
                 (currentTask.Id,
                 currentTask.Description,
                 currentTask.Alias,
                 currentTask.CreatedAtDate,
                 currentTask.RequiredEffort,
                 currentTask.IsMilestone,
                 currentTask.StartDate,
                 ScheduleTime,
                 currentTask.DeadlineDate,
                 currentTask.CompleteDate,
                 currentTask.Deliverables,
                 currentTask.Remarks,
                 currentTask.EngineerId,
                 currentTask.ComplexityLevel));
            }
            DO.Task newEndTask = _dal.Task.Read(currentTask.Id) ?? throw new BO.BlNullPropertyException("id Of Task can't be null");
            SetScheduledDateForTask(newEndTask, endTask);

        }
    }

    /// <summary>
    /// A function that calculates completion times for each of the tasks in the project
    /// </summary>
    private void SetDeadLineDateForTask(DO.Task endTask, DO.Task startTask)
    {
        if (endTask.Id == startTask.Id)
            return;

        var allDependencies = _dal.Dependency
            .ReadAll()
            .Where(dependency => dependency != null);

        var DependsOnTaskList = allDependencies.Where(dep => dep?.DependentTask == endTask.Id)
            .Select(dep => dep?.DependsOnTask).ToList();

        foreach (var dep in DependsOnTaskList)
        {
            //calling the pending task
            DO.Task currentTask = _dal.Task.Read(dep!.Value) ?? throw new BO.BlNullPropertyException("id Of Task can't be null");
            // task completion time - how long it takes = the completion time depends
            DateTime? deadlineTime = endTask.DeadlineDate - endTask.RequiredEffort;
            if (currentTask.DeadlineDate == null || currentTask.DeadlineDate > deadlineTime)
            {
                _dal.Task.Update(
                  new DO.Task
                  (currentTask.Id,
                  currentTask.Description,
                  currentTask.Alias,
                  currentTask.CreatedAtDate,
                  currentTask.RequiredEffort,
                  currentTask.IsMilestone,
                  currentTask.StartDate,
                  currentTask.ScheduledDate,
                  deadlineTime,
                  currentTask.CompleteDate,
                  currentTask.Deliverables,
                  currentTask.Remarks,
                  currentTask.EngineerId,
                  currentTask.ComplexityLevel));
            }

            DO.Task newEndTask = _dal.Task.Read(currentTask.Id) ?? throw new BO.BlNullPropertyException("id Of Task can't be null");
            //A call in recursion to each of the tasks in the list to calculate its completion time according to the algorithm
            SetDeadLineDateForTask(newEndTask, startTask);

        }
    }

    /// <summary>
    /// We will update the automatic initial nicknames of the milestones to meaningful names, according to the names of the tasks.
    /// </summary>
    private void setNameOfMilestone()
    {
        string newAlias = "";
        var milstoneList = _dal.Task.ReadAll(task => task.IsMilestone).Where(t => t != null).ToList();
        foreach (var milstone in milstoneList)
        {
            if (milstone!.Alias != "End" && milstone.Alias != "Start")
            {
                var dependencies = _dal.Dependency.ReadAll(task => task.DependentTask == milstone!.Id)
                                              .Where(t => t != null)
                                              .Select(dep => dep?.DependsOnTask)
                                              .ToList();
                newAlias = "";
                foreach (var dependency in dependencies)
                {
                    if (dependency != null)
                    {
                        DO.Task task = _dal.Task.Read((int)dependency)!;
                        newAlias = newAlias + ", " + task.Alias;
                    }
                }
                _dal.Task.Update(
                   new DO.Task
                   (milstone!.Id,
                       milstone.Description,
                       newAlias,
                       milstone.CreatedAtDate,
                       milstone.RequiredEffort,
                       milstone.IsMilestone,
                       milstone.StartDate,
                       milstone.ScheduledDate,
                       milstone.DeadlineDate,
                       milstone.CompleteDate,
                       milstone.Deliverables,
                       milstone.Remarks,
                       milstone.EngineerId,
                       milstone.ComplexityLevel));
            }
        }
    }
}