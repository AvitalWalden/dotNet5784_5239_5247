namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int id = item.Id;
        XElement tasksElement = XElement.Load("path/to/tasks.xml");

        if (tasksElement.Elements("Task").Any(e => (int)e.Element("Id") == id))
            throw new DalAlreadyExistsException($"Task with ID={id} already exists");

        XElement newTaskElement = new XElement("Task",
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

        );
        tasksElement.Add(newTaskElement);
        tasksElement.Save("path/to/tasks.xml");
        return id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public void Update(Task item)
    {
        int id = item.Id;
        // call to XML file
        XDocument xdoc = XDocument.Load("path/to/tasks.xml");
        // Check if there is an engineer with the same ID
        XElement? taskToUpdate = xdoc.Descendants("Task").FirstOrDefault(e => (int)e.Element("Id") == id);
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
            xdoc.Save("path/to/tasks.xml");
        }
        else
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");
        }
    }
}
