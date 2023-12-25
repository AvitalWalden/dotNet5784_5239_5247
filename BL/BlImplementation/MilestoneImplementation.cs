using BlApi;
using DalApi;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Milestone item)
    {
        // יצירת רשימה מקובצת על פי מפתח - משימה תלויה
        var groupedDependencies = _dal.Dependency.ReadAll()
            .Select(dependency => new { TaskId = dependency.DependentTask, DependsOnTaskId = dependency.DependsOnTask })
            .GroupBy(dep => dep.DependsOnTaskId)
            .OrderBy(group => group.Key)
            .ToList();

        // יצירת רשימה מסוננת שבה כל ערך מופיע רק פעם אחת
        var distinctDependencies = groupedDependencies
            .Select(group => new { TaskId = group.Key, Dependencies = group.Select(dep => dep.TaskId).Distinct().ToList() })
            .ToList();

        // יצירת אבני דרך
        var milestones = distinctDependencies.Select(dep => new { MilestoneId = dep.TaskId, Dependencies = dep.Dependencies }).ToList();

        // הוספת תלות נוספות עבור כל משימה שלא תלויה באף משימה קודמת
        var projectStartMilestone = new { MilestoneId = 0, Dependencies = new List<int>() };
        projectStartMilestone.Dependencies.AddRange( _dal.Dependency.ReadAll().Select(task => task.Id));
        milestones.Insert(0, projectStartMilestone);

        // עכשיו אתה יכול לעבוד עם רשימת האבנים והתלויות החדשה
        Console.WriteLine("Milestones and Dependencies:");
        foreach (var milestone in milestones)
        {
            Console.WriteLine($"Milestone {milestone.MilestoneId}: {string.Join(", ", milestone.Dependencies)}");
        }
        throw new NotImplementedException();
    }

    public BO.Milestone? Read(int id)
    {
        throw new NotImplementedException();
    }

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
        //DO.Task doMilestone = new DO.Task(boMilestone.Id, boMilestone.Description, boMilestone.Alias,boMilestone.CreatedAtDate, boMilestone.Status);
        //try
        //{
        //    _dal.Task.Update(doMilestone);
        //}
        //catch (DO.DalDoesNotExistException)
        //{
        //    throw new BO.BlDoesNotExistException($"Task with ID={boMilestone.Id}  does not exist");
        //}
    }
}
