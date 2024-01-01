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
        if (startDate != null && completeDate > DateTime.Now)
            return BO.Status.Done;

        return BO.Status.Unscheduled;
    }

    public static void SetProjectDates(DateTime startDate, DateTime endDate)
    {
        DalApi.Factory.Get.endDateProject = endDate;
        DalApi.Factory.Get.startDateProject = startDate;

    }
}
