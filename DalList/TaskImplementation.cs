namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;

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
        Task? taskToDelete = Read(id);
        if (taskToDelete is not null)
        {
            if (DataSource.Dependencies.Any(dependency => dependency?.DependsOnTask == id))
                throw new DalDeletionImpossible($"Task with ID={id} cannot be deleted");

            DataSource.Dependencies.RemoveAll(dependency => dependency?.DependentTask == id);
            DataSource.Tasks.Remove(taskToDelete);
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        }
        //Task? taskToDelete = Read(id);
        //if (taskToDelete is not null)
        //{
        //    taskToDelete.Active = false;
        //}
        // Throw an exception if the task cannot be deleted
        //   throw new DalDeletionImpossible($"Task with ID={id} cannot be deleted");
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
