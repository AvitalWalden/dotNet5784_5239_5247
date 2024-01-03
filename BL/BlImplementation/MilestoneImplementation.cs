using BlApi;
using BO;
using DalApi;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create()
    {
        var groupedDependencies = _dal.Dependency.ReadAll()
            .OrderBy(dep => dep?.DependsOnTask)
            .GroupBy(dep => dep?.DependentTask, dep => dep?.DependsOnTask,
            (id, dependency) => new { TaskId = id, Dependencies = dependency })
            .ToList();

        var distinctDependencies = groupedDependencies
            .Select(group => group.Dependencies.Distinct().ToList())
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

                foreach (var idTask in dep)
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
                    // אם הרשימה של המשימה (DependentTask) שווה לרשימה של המשימה הנוצרת
                    if (dependencyGroup.Dependencies.SequenceEqual(dep))
                    {
                        foreach (var dependentOnTaskId in dependencyGroup.Dependencies)
                        {
                            dependencies.Add(new DO.Dependency
                            {
                                DependentTask = dependentOnTaskId!.Value,
                                DependsOnTask = milestoneId
                            });
                        }
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
                scheduledDate = _dal.Task.ReadAll(task => _dal.Dependency.ReadAll().Any(dependency => dependency?.DependentTask == task.Id && dependency.DependsOnTask == milstone.Id)).Min(t => t!.ScheduledDate!); //.value
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
                DeadlineDate = _dal.Task.ReadAll(task => _dal.Dependency.ReadAll().Any(dependency => dependency?.DependentTask == task.Id && dependency.DependsOnTask == milstone.Id)).Max(t => t!.DeadlineDate!); //.value
            }
        }

    }

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
}