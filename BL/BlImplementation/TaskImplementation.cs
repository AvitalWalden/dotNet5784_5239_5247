using BlApi;
using BO;
using DO;
using System.Reflection.Emit;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task boTask)
    {
        //if (boTask.Id < 0)
        //{
        //    throw new Exception("Task ID must be a positive number");
        //}
        //if (string.IsNullOrWhiteSpace(boTask.Description))
        //{
        //    throw new Exception("Task description cannot be empty or null");
        //}
        //DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.CreatedAtDate, boTask.DeadlineDate, );
        //try
        //{
        //    int idTask = _dal.Task.Create(doTask);
        //    return idTask;
        //}
        //catch (DO.DalAlreadyExistsException ex)
        //{
        //    throw new BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        //}
        return 0;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.TaskInList> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task item)
    {
        throw new NotImplementedException();
    }
}
