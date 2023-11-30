﻿namespace Dal;
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
        const string tasksFile = @"..\xml\tasks.xml";
        int id = Config.NextTaskId;
        XElement tasksElement = XElement.Load(tasksFile);  // למה לא XElement XDocument

        if (tasksElement.Elements("Task").Any(e => (int)e.Element("Id")! == id))
            throw new DalAlreadyExistsException($"Task with ID={id} already exists");

        XElement newTaskElement = new XElement("Task",
            new XElement("Id", Config.NextTaskId),
            new XElement("Description", item.Description),
            new XElement("Alias", item.Alias),
            new XElement("Milestone", item.Milestone),
            new XElement("Start", item.Start),
            new XElement("ForecastDate", item.ForecastDate),
            new XElement("Deadline", item.Deadline),
            new XElement("Complete", item.Complete),
            new XElement("Deliverables", item.Deliverables),
            new XElement("Remarks", item.Remarks),
            new XElement("EngineerId", item.EngineerId),
            new XElement("ComplexityLevel", item.ComplexityLevel),
            new XElement("Active", item.Active),
            new XElement("CreatedAt", item.CreatedAt)
        );
        tasksElement.Add(newTaskElement);
        tasksElement.Save(tasksFile);
        return id;
    }

    public void Delete(int id)
    {
        throw new DalDeletionImpossible($"Task with ID={id} cannot be deleted");
    }

    public Task? Read(Func<Task, bool> filter)
    {
        throw new NotImplementedException();
    }

    public Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        //    List<Task> lst = new List<Task>();

        //    XmlSerializer ser = new XmlSerializer(typeof(List<Task>));

        //    using (StreamReader r = new StreamReader(@"..\xml\tasks.xml"))
        //    {
        //        lst = (List<Task>)ser.Deserialize(r)!;
        //        r.Close();
        //    }

        //    //return (func == null) ? lst : lst?.Where(func);

        //    return lst;
        throw new NotImplementedException();

    }

    public void Update(Task item)
    {
        int id = item.Id;
        // call to XML file
        const string tasksFile = @"..\xml\tasks.xml";
        XDocument? xdoc = XDocument.Load(tasksFile);
        // Check if there is an engineer with the same ID
        XElement? taskToUpdate = xdoc.Descendants("Task").FirstOrDefault(e => (int)e.Element("Id")! == id);
        if (taskToUpdate != null)
        {
            // Update the task in the XML file
            taskToUpdate.ReplaceWith(
                new XElement("Task",
                new XElement("Id", item.Id),
                new XElement("Description", item.Description),
                new XElement("Alias", item.Alias),
                new XElement("Milestone", item.Milestone),
                new XElement("Start", item.Start),
                new XElement("ForecastDate", item.ForecastDate),
                new XElement("Deadline", item.Deadline),
                new XElement("Complete", item.Complete),
                new XElement("Deliverables", item.Deliverables),
                new XElement("Remarks", item.Remarks),
                new XElement("EngineerId", item.EngineerId),
                new XElement("ComplexityLevel", item.ComplexityLevel),
                new XElement("Active", item.Active)
                )
            );
            // Save to XML file again
            xdoc.Save(tasksFile);
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");
        }
    }
}
