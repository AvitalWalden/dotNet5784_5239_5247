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
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        //return new BO.Task()
        //{
        //    Id = id,
        //    Description = doTask.Description,
        //    Alias = doTask.Alias,
        //    CreatedAtDate = doTask.CreatedAtDate,
        //    Status = 
        //    Cost = doTask.Cost,
        //    Task = new BO.TaskInEngineer()
        //    {
        //        Id = (int)(_dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doEngineer.Id)?.Id!),
        //        Alias = _dal.Task.ReadAll().FirstOrDefault(task => task?.EngineerId == doEngineer.Id)?.Alias!
        //    }
        //};
        throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

    }

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task item)
    {
        throw new NotImplementedException();
    }
}
