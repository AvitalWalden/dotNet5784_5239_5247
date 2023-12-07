namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.Xml.Serialization;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks"); 
        int id = XMLTools.GetAndIncreaseNextId("data-config", "NextTaskId");
        Task copy = item with { Id = id };
        lst.Add(copy);
        XMLTools.SaveListToXMLSerializer<Task>(lst, "tasks");
        return id;
    }

    public void Delete(int id)
    {
        throw new DalDeletionImpossible($"Task with ID={id} cannot be deleted");
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        if (filter != null)
        {
            return lst.Where(filter).FirstOrDefault();
        }
        return lst.FirstOrDefault(tasks => filter!(tasks!));
    }

    public Task? Read(int id)
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        return lst.FirstOrDefault(tasks => tasks?.Id == id);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        return XMLTools.LoadListFromXMLSerializer<Task>("tasks");
    }

    public void Update(Task item)
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = lst.FirstOrDefault(task => task?.Id == item.Id);
        if (task is null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} is not exists");
        lst.Remove(task);
        lst.Add(item);
        XMLTools.SaveListToXMLSerializer<Task>(lst, "tasks");
    }

    public void Reset()
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        lst.Clear();
        XMLTools.SaveListToXMLSerializer<Task>(lst, "tasks");
    }
}
