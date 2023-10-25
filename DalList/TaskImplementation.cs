namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        //for entities with auto id
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.Dependencies.Count; i++)
        {
            if (DataSource.Dependencies[i].DependentTask == id || DataSource.Dependencies[i].DependsOnTask == id)
            {
                throw new Exception($"Task with ID={id} cannot be deleted");
            }
        }
        if (Read(id) is not null)
        {
            DataSource.Tasks.RemoveAt(id);
        }
        else
        {
            throw new Exception($"Task with ID={id} not exists");
        }
    }

    public Task? Read(int id)
    {
        if (DataSource.Tasks.Exists(task => task.Id == id))
        {
            Task? task = DataSource.Tasks.Find(task => task.Id == id);
            if (task is not null)
            {
                return task;
            }
        }
        return null;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (DataSource.Tasks.Exists(task => task.Id == item.Id))
        {
            Task? task = DataSource.Tasks.Find(task => task.Id == item.Id);
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
