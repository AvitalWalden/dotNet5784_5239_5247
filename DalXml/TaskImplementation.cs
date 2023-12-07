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
        const string tasksFile = @"..\xml\tasks.xml";
        XmlSerializer serializer = new XmlSerializer(typeof(List<Task>));
        TextReader textReader = new StringReader(tasksFile);
        List<Task> lst = (List<Task>?)serializer.Deserialize(textReader) ?? throw new Exception();
        lst.Add(item);
        using (TextWriter writer = new StreamWriter(tasksFile))
        {
            serializer.Serialize(writer, lst);
        }
        return item.Id;
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
        // Use an absolute path or ensure the relative path is correct
        string filePath = @"..\xml\tasks.xml";

        // Create an XmlSerializer for the Engineer type
        XmlSerializer serializer = new XmlSerializer(typeof(List<Task>));

        // Read the XML data from the file
        using (StreamReader reader = new StreamReader(filePath))
        {
            // Deserialize the XML data into a List<Engineer>
            List<Task> tasks = (List<Task>)serializer.Deserialize(reader)!;

            // Apply the filter if provided
            if (filter != null)
            {
                tasks = tasks.Where(filter).ToList();
            }

            return tasks;
        }

    }

    public void Update(Task item)
    {
       
    }
}
