using System.Reflection;

namespace BO;

public  static class Tools
{
    public static string ToStringProperty<T>(this T obj)
    {
        PropertyInfo[] properties = typeof(T).GetProperties(); //Get all properties of T.

        string result = string.Join(", ", properties.Select(property =>
        {
            object? value = property.GetValue(obj);
            string? valueString = (value != null) ? value.ToString() : "null";
            return $"{property.Name}: {valueString}";
        }));

        return result;
    }
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
        if (doTask.StartDate != null && doTask.CompleteDate!=null && doTask.CompleteDate < DateTime.Now)
            return BO.Status.Done;

        return BO.Status.Unscheduled;
    }

    public static void SetProjectDates(DateTime startDate, DateTime endDate)
    {
        DalApi.Factory.Get.endDateProject = endDate;
        DalApi.Factory.Get.startDateProject = startDate;

    }
}
