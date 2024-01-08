namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;
using static Dal.DataSource;

internal class TaskImplementation : ITask
{
    // This method is used to create a new Task
    public int Create(Task item)
    {
        //for entities with auto id
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    // This method is used to delete a Task by ID
    public void Delete(int id)
    {
        //מחיקה מה לעשות בBO אם כאן עושים את זה
        Task? taskToDelete = Read(id);
        var isMilstone = ReadAll(task => task.IsMilestone);
        if (isMilstone.Count() > 0)
            throw new DalDeletionImpossible("Task cannot be deleted because the project already began");
        if (taskToDelete is not null)
        {
            for (int i = 0; i < DataSource.Dependencies.Count; i++)
            {
                if (DataSource.Dependencies[i]?.DependsOnTask == id)
                {
                    throw new Exception($"Task with ID ={id} cannot be deleted");
                }
            }
            for (int i = 0; i < DataSource.Dependencies.Count; i++)
            {
                if (DataSource.Dependencies[i]?.DependentTask == id)
                {
                    DataSource.Dependencies.Remove(DataSource.Dependencies[i]);
                }
            }
            DataSource.Tasks.Remove(taskToDelete);
        }
        else
        {
            throw new Exception($"Task with ID={id} not exists");
        }
    }

    //Reads task object by filter function
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(task => filter(task!));
    }

    // This method is used to read a Task by ID
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(task => task?.Id == id);

    }

    // This method is used to read all Tasks
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        // If no filter is provided, return all tasks
        return from item in DataSource.Tasks
               select item;
    }

    // This method is used to update the task 
    public void Update(Task item)
    {
        Task? taskToUpdate = DataSource.Tasks.FirstOrDefault(task => task?.Id == item.Id);
        if (taskToUpdate is not null)
        {
            DataSource.Tasks.Remove(taskToUpdate);
            DataSource.Tasks.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");
        }
    }

    public void Reset()
    {
        if (DataSource.Tasks.Any())
        {
            DataSource.Tasks.Clear();
        }
    }
}
