namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
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