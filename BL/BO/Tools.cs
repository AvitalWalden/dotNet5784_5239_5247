using BlApi;
using DO;
using System.Collections;
using System.Reflection;

namespace BO;

public static class Tools
{
    /// <summary>
    /// function that print the object
    /// </summary>
    public static string ToStringProperty<T>(this T obj)
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        string result = string.Join(", ", properties.Select(property =>
        {
            object? value = property.GetValue(obj);
            string? valueString;

            if (value == null)
            {
                valueString = "null";
            }
            else if (value is IEnumerable enumerableValue)
            {
                valueString = string.Join("", enumerableValue.Cast<object>().Select(item => item.ToString()));
            }
            else
            {
                valueString = value.ToString()!;
            }

            return $" {property.Name}: {valueString}";
        }));

        return result;
    }

    /// <summary>
    /// calculate the status of the task
    /// </summary>
    /// <param name="doTask">the task to calculate</param>
    /// <returns></returns>
    public static BO.Status CalculateStatusOfTask(DO.Task doTask)
    {
        if (doTask.StartDate == null && doTask.DeadlineDate == null)
            return BO.Status.Unscheduled;

        if (doTask.StartDate != null && doTask.DeadlineDate != null && doTask.CompleteDate == null)
            return BO.Status.Scheduled;

        if (doTask.StartDate != null && doTask.CompleteDate != null && doTask.CompleteDate <= doTask.ScheduledDate)
            return BO.Status.OnTrack;

        if (doTask.StartDate != null && doTask.CompleteDate != null && doTask.CompleteDate > doTask.ScheduledDate)
            return BO.Status.InJeopardy;
        if (doTask.StartDate != null && doTask.CompleteDate != null && doTask.CompleteDate < DateTime.Now)
            return BO.Status.Done;

        return BO.Status.Unscheduled;
    }

    public static void SetProjectDates(DateTime startDate, DateTime endDate)
    {
        DalApi.Factory.Get.endDateProject = endDate;
        DalApi.Factory.Get.startDateProject = startDate;

    }

    /// <summary>
    /// Calculate TaskInList for the task with tjis id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the dependencies of this task</returns>
    public static List<BO.TaskInList> CalculateTaskInList(int id)
    {
        DalApi.IDal _dal = DalApi.Factory.Get;

        List<BO.TaskInList> tasksList = new List<BO.TaskInList>();
        _dal.Dependency.ReadAll(dependency => dependency.DependentTask == id)
                           .Select(dependency => _dal.Task.Read(task => task.Id == dependency?.DependsOnTask))
                           .ToList()
                           .ForEach(task =>
                           {
                               if (task != null)
                               {
                                   tasksList.Add(new BO.TaskInList()
                                   {
                                       Id = task.Id,
                                       Alias = task.Alias,
                                       Description = task.Description,
                                       Status = (BO.Status)Tools.CalculateStatusOfTask(task)
                                   });
                               }
                           });
        return tasksList;
    }
}
