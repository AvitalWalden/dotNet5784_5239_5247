using BlApi;
using BO;
using DalApi;
using System.Numerics;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Milestone item)
    {
      
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
        DO.Task doMilestone;// = new DO.Task(boMilestone.Id, boMilestone.Description, boMilestone.Alias, boMilestone.CreatedAtDate );////boMilestone.Status
        try
        {
          //  _dal.Task.Update(doMilestone);
        }
        catch (DO.DalDoesNotExistException)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boMilestone.Id}  does not exist");
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




