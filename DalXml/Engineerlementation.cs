namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class Engineerlementation : IEngineer
{
    public int Create(Engineer item)
    {
        int id = item.Id;
        const string engineersFile = @"..\xml\engineers.xml";
        XElement engineersElement = XElement.Load(engineersFile);

        if (engineersElement.Elements("Engineer").Any(e => (int)e.Element("Id")! == id))
            throw new DalAlreadyExistsException($"Engineer with ID={id} already exists");

        XElement newEngineerElement = new XElement("Engineer",
            new XElement("Id", item.Id),
            new XElement("Name", item.Name),
            new XElement("Email", item.Email),
            new XElement("Level", item.Level),
            new XElement("Cost", item.Cost),
            new XElement("Active", item.Active)
        );
        engineersElement.Add(newEngineerElement);
        engineersElement.Save(engineersFile);
        return id;
    }

    public void Delete(int id)
    {
        throw new DalDeletionImpossible($"Engineer with ID={id} cannot be deleted");
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer item)
    {
        int id = item.Id;
        // call to XML file
        const string engineersFile = @"..\xml\engineers.xml";
        XDocument xdoc = XDocument.Load(engineersFile);
        // Check if there is an engineer with the same ID
        XElement? engineerToUpdate = xdoc.Descendants("Engineer").FirstOrDefault(e => (int)e.Element("Id")! == id);
        if (engineerToUpdate != null)
        {
            // Update the engineer in the XML file
            engineerToUpdate.ReplaceWith(
                new XElement("Engineer",
                    new XElement("Id", item.Id),
                    new XElement("Name", item.Name),
                    new XElement("Email", item.Email),
                    new XElement("Level", item.Level),
                    new XElement("Cost", item.Cost),
                    new XElement("Active", item.Active)
                )
            );
                        // Save to XML file again
            xdoc.Save(engineersFile);
        }
        else
        {
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");
        }
    }
}
