namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = item.Id;
        XElement dependenciesElement = XElement.Load("path/to/dependecies.xml");

        if (dependenciesElement.Elements("Dependency").Any(e => (int)e.Element("Id") == id))
            throw new DalAlreadyExistsException($"Dependency with ID={id} already exists");

        XElement newDependencyElement = new XElement("Dependency",
            new XElement("Id", item.Id),
            new XElement("Description", item.DependentTask),
            new XElement("Alias", item.DependsOnTask)
            );
        dependenciesElement.Add(newDependencyElement);
        dependenciesElement.Save("path/to/dependecies.xml");
        return id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}

