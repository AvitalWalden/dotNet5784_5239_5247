namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;

public class TaskImplementation : ITask
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
        if (Read(id) is not null)
        {
            for (int i = 0; i < DataSource.Dependencies.Count; i++)
            {
                if (DataSource.Dependencies[i]?.DependsOnTask == id)
                {
                   throw new Exception($"Task with ID={id} cannot AVITAL be deleted");
                }
            }
            for (int i = 0;i < DataSource.Dependencies.Count;i++)
            {
                if (DataSource.Dependencies[i]?.DependentTask == id)
                {
                    DataSource.Dependencies.RemoveAt(DataSource.Dependencies[i]!.Id);
                }
            }
            DataSource.Tasks.RemoveAt(id);
        }
        else
        {
            throw new Exception($"Task with ID={id} not exists");
        }
    }

    // This method is used to read a Task by ID
    public Task? Read(int id)
    {
        if (DataSource.Tasks.Exists(task => task?.Id == id))
        {
            Task? task = DataSource.Tasks.Find(task => task?.Id == id);
            return task;
        }
        return null;
    }

    // This method is used to read all Tasks
    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks!);
    }

    // This method is used to update the task 
    public void Update(Task item)
    {
        if (DataSource.Tasks.Exists(task => task?.Id == item.Id))
        {
            Task? task = DataSource.Tasks.Find(task => task?.Id == item.Id);
            if (task is not null)
            {
                DataSource.Tasks.Remove(task);
                DataSource.Tasks.Add(item);
            }
        }
        else
        {
            throw new Exception($"Task with ID={item.Id} not exists");
        }
    }
}
