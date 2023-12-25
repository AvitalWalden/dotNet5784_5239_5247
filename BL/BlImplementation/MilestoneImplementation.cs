using BlApi;
using BO;
using DalApi;

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
     //   DO.Task doMilestone = new DO.Task(boMilestone.Id, boMilestone.Alias, boMilestone.Description,boMilestone.CreatedAtDate,/////////////);
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
