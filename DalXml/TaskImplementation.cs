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
    /// <summary>
    /// Method to create a new task
    /// </summary>
    /// <param name="item">task details to add</param>
    /// <returns>the new id of task</returns>
    public int Create(Task item)
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks"); 
        int id = XMLTools.GetAndIncreaseNextId("data-config", "NextTaskId");
        Task copy = item with { Id = id };
        lst.Add(copy);
        XMLTools.SaveListToXMLSerializer<Task>(lst, "tasks");
        return id;
    }

    /// <summary>
    ///  Method to delete an task by ID
    /// </summary>
    /// <param name="id">ID of task you want to delete</param>
    /// <exception cref="DalDeletionImpossible">exeption that task cannot be deleted</exception>
    public void Delete(int id)
    {
        List<Dependency> lstDependency = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencies");
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        Task? task = lst.FirstOrDefault(task => task?.Id == id);
        if (task is null)
            throw new DalDoesNotExistException($"Task with ID={id} is not exists");
        if (Config.startDateProject >= DateTime.Now)
            throw new DalDeletionImpossible("Task cannot be deleted because the project alredy began");
        foreach (var dep in lstDependency)
        {
            if (dep.DependsOnTask == id)
            {
                throw new Exception($"Task with ID ={id} cannot be deleted");
            }
        }
        foreach (var dep in lstDependency)
        {
            if (dep.DependentTask == id)
            {
                lstDependency.Remove(dep);
            }
        }
        lst.Remove(task);
        XMLTools.SaveListToXMLSerializer<Task>(lst, "tasks");
    }

    /// <summary>
    /// Method to read an task using a custom filter
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>Returns task according to ID and according to the existence of the filter</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        if (filter != null)
        {
            return lst.Where(filter).FirstOrDefault();
        }
        return lst.FirstOrDefault(tasks => filter!(tasks!));
    }

    /// <summary>
    ///   Method to read an task by ID
    /// </summary>
    /// <param name="id">ID of task you want to read</param>
    /// <returns>Task by ID</returns>
    public Task? Read(int id)
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        return lst.FirstOrDefault(tasks => tasks?.Id == id);
    }

    /// <summary>
    /// Method to read all tasks with an optional filter
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>read all Dependency, or read all task that remain after the filter function</returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        return XMLTools.LoadListFromXMLSerializer<Task>("tasks");
    }

    /// <summary>
    /// Method to update an existing task
    /// </summary>
    /// <param name="item">Task details to update</param>
    /// <exception cref="DalDoesNotExistException">If the task does not exists</exception>
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

    /// <summary>
    ///  Method to reset the list of task.
    /// </summary>
    public void Reset()
    {
        List<Task> lst = XMLTools.LoadListFromXMLSerializer<Task>("tasks");
        if (lst.Any())
        {
            lst.Clear();
            XMLTools.SaveListToXMLSerializer<Task>(lst, "tasks");
        }
        string configFile = "data-config";
        XElement configElement = XMLTools.LoadListFromXMLElement(configFile);
        configElement.Element("NextTaskId")?.SetValue("0");
        XMLTools.SaveListToXMLElement(configElement, configFile);
    }
}
