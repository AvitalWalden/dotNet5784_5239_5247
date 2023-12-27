using BlApi;
using BO;
using DalApi;
using System.Numerics;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(List<BO.TaskInList> Dependency)
    {
        // יצירת רשימה מקובצת על פי מפתח - משימה תלויה
        var groupedDependencies = _dal.Dependency.ReadAll()
            .OrderBy(dep => dep?.DependsOnTask)
            .GroupBy(dep => dep?.DependentTask, dep => dep?.DependsOnTask, (id, dependency)=> new { TaskId = id, Dependencies = dependency })
            .ToList();

        // יצירת רשימה מסוננת שבה כל ערך מופיע רק פעם אחת
        var distinctDependencies = groupedDependencies
         .SelectMany(depGroup => depGroup.Dependencies)
         .Where(dep => dep != null)
         .Distinct()
         .ToList();

        // יצירת אבן דרך עבור כל ערך ברשימת התלויות החדשה
        var milestones = distinctDependencies
            .Select(dep => new BO.Milestone
            {
                Id = dep.Value, // השמה של מזהה המשימה התלויה
                Description = $"Milestone for Task {dep.Value}", // תיאור אוטומטי
                Alias = $"M{dep.Value}", // קיצור אוטומטי
                CreatedAtDate = DateTime.Now, // זמן יצירה
                Status = BO.Status.Scheduled, // סטטוס עבור אבן דרך חדשה
                StartDate = null, // תאריך התחלה - אפשר להשיב בהתאם לדרישות העסקיות
                ForecastDate = null, // תאריך תחזית - אפשר להשיב בהתאם לדרישות העסקיות
                DeadlineDate = null, // תאריך סיום אחרון - אפשר להשיב בהתאם לדרישות העסקיות
                CompleteDate = null, // תאריך סיום בפועל - אפשר להשיב בהתאם לדרישות העסקיות
                Dependencies = groupedDependencies
                    .Where(depGroup => depGroup.Dependencies.Contains(dep))
                    .Select(depGroup =>
                    {
                        return new BO.TaskInList()
                        {
                            Id = (int)depGroup.TaskId,
                            Alias = $"Task-{depGroup.TaskId}",
                            Description = $"Task {depGroup.TaskId}", // תיאור אוטומטי
                            Status = BO.Status.Scheduled // סטטוס עבור המשימה התלויה

                        };
                    })
                    .ToList()
            })
            .ToList();


        // יצירת אבן דרך לתחילת הפרויקט
        var projectStartMilestone = new BO.Milestone
        {
            Id = 0, // ערך ייחודי לאבן הדרך שמייצגת את תחילת הפרויקט
            Description = "Project Start Milestone", // תיאור אוטומטי
            Alias = "M0", // קיצור אוטומטי
            CreatedAtDate = DateTime.Now, // זמן יצירה
            Status = BO.Status.Scheduled, // סטטוס עבור אבן דרך חדשה
            StartDate = null, // תאריך התחלה - אפשר להשיב בהתאם לדרישות העסקיות
            ForecastDate = null, // תאריך תחזית - אפשר להשיב בהתאם לדרישות העסקיות
            DeadlineDate = null, // תאריך סיום אחרון - אפשר להשיב בהתאם לדרישות העסקיות
            CompleteDate = null, // תאריך סיום בפועל - אפשר להשיב בהתאם לדרישות העסקיות
            Dependencies = new List<BO.TaskInList>() // לא יש לה תלות
        };

        // הוספת אבן הדרך לרשימה
        milestones.Add(projectStartMilestone);

        // מציאת משימות שלא תלויות בשום משימה
        var independentTasks = _dal.Task.ReadAll(task => !groupedDependencies.Any(depGroup => depGroup.Dependencies.Contains(task.Id)));

        // הוספת תלות לכל אבן דרך עבור המשימות הזו
        foreach (var independentTask in independentTasks)
        {
            projectStartMilestone.Dependencies.Add(new BO.TaskInList
            {
                Id = independentTask.Id,
                Alias = $"Task-{independentTask.Id}",
                Description = $"Task {independentTask.Id} - {independentTask.Description}",
                Status = BO.Status.Scheduled
            });
        }
    }

    public BO.Milestone? Read(int id)
    {
        DO.Task? doTaskMilestone = _dal.Task.Read(task => task.Id == id && task.IsMilestone);
        if (doTaskMilestone == null)
            throw new BO.BlDoesNotExistException($"Milstone with ID={id} does Not exist");

        var tasksId = _dal.Dependency.ReadAll(dependensy => dependensy.DependsOnTask == doTaskMilestone.Id)
                                     .Select(dependensy => dependensy?.DependentTask);

        var tasks = _dal.Task.ReadAll(task => tasksId.Contains(task.Id)).ToList();

        var tasksInList = tasks.Select(task => new BO.TaskInList
        {
            Id = task.Id,
            Description = task.Description,
            Alias = task.Alias,
            Status = CalculateStatus(task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate)
        }).ToList();

        return new BO.Milestone()
        {
            Id = doTaskMilestone.Id,
            Description = doTaskMilestone.Description,
            Alias = doTaskMilestone.Alias,
            CreatedAtDate = doTaskMilestone.CreatedAtDate,
            Status = CalculateStatus(doTaskMilestone.StartDate, doTaskMilestone.ScheduledDate, doTaskMilestone.DeadlineDate, doTaskMilestone.CompleteDate),
            ForecastDate = doTaskMilestone.ScheduledDate,
            DeadlineDate = doTaskMilestone.DeadlineDate,
            CompleteDate = doTaskMilestone.CompleteDate,
            CompletionPercentage = (tasksInList.Count(task => task.Status == Status.OnTrack) / (double)tasksInList.Count) * 100,
            Remarks = doTaskMilestone.Remarks,
            Dependencies = tasksInList
        };
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
    public Status CalculateStatus(DateTime? startDate, DateTime? forecastDate, DateTime? deadlineDate, DateTime? completeDate)
    {
        if (startDate == null && deadlineDate == null)
            return Status.Unscheduled;

        if (startDate != null && deadlineDate != null && completeDate == null)
            return Status.Scheduled;
        if (startDate != null && completeDate != null && completeDate <= forecastDate)
            return Status.OnTrack;
        if (startDate != null && completeDate != null && completeDate > forecastDate)
            return Status.InJeopardy;

        return Status.Unscheduled;
    }

}