namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;
        const string dependenciesFile = @"..\xml\dependencies.xml";
        //XElement dependenciesElement = XElement.Load(dependenciesFile); // למה אין כאן .ROOT והאם צריך ?
        XElement? allDependencies = XDocument.Load(dependenciesFile).Root;

        if (allDependencies!.Elements("Dependency").Any(dependency => (int)dependency.Element("Id")! == id)) //האם אפשר לשים !
            throw new DalAlreadyExistsException($"Dependency with ID={id} already exists");

        XElement newDependencyElement = new XElement("Dependency",
            new XElement("Id", id),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependsOnTask", item.DependsOnTask)
            );
        allDependencies.Add(newDependencyElement);
        allDependencies.Save(dependenciesFile);
        return id;
    }

    public void Delete(int id)
    {

        XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;
        XElement? deleteDependency = allDependencies?.Elements().ToList().Find(dependency => Convert.ToInt32(dependency?.Element("Id")?.Value) == id);
        if (deleteDependency == null)
        {
            throw new DalDoesNotExistException($"dependency with ID={id} not exists");
        }
        deleteDependency!.Remove();
        allDependencies?.Save(@"..\xml\dependencies.xml");
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;

        XElement? dependencyElement = allDependencies?
                    .Elements("Dependency")
                    .FirstOrDefault(dependency => filter(new Dependency(
                        (int)dependency.Element("Id")!,
                        (int)dependency.Element("DependentTask")!,
                        (int)dependency.Element("DependsOnTask")!
                    )));

        if (dependencyElement != null)
        {
            Dependency? dependency = new Dependency(
                (int)dependencyElement.Element("Id")!,
                (int)dependencyElement.Element("DependentTask")!,
                (int)dependencyElement.Element("DependsOnTask")!
            );
            return dependency;
        }
        return null;
    }

    public Dependency? Read(int id)
    {
        XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;

        XElement? dependencyElement = allDependencies?
                    .Elements("Dependency")
                    .FirstOrDefault(dependency => (int)dependency?.Element("Id")! == id); //האם אפשר לשים !

        if (dependencyElement != null)
        {
            Dependency? dependency = new Dependency((int)dependencyElement.Element("Id")!, (int)dependencyElement.Element("DependentTask")!, (int)dependencyElement.Element("DependsOnTask")!);
            return dependency;
        }
        return null;
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement? dependenciesElement = XMLTools.LoadListFromXMLElement("ArrayOfDependency");
        IEnumerable<Dependency> dependencies = dependenciesElement
            .Elements("Dependency")
            .Select(e => new Dependency(
                Id: (int)e.Element("id")!,
                DependentTask: (int)e.Element("dependentTask")!,
                DependsOnTask: (int)e.Element("dependsOnTask")!
            ));

        if (filter != null)
        {
            dependencies = dependencies.Where(filter);
        }

        return dependencies.ToList(); // Convert to List before returning

        //if (filter != null)
        //{
        //    return from item in DataSource.Dependencies
        //           where filter(item)
        //           select item;
        //}
        //// If no filter is provided, return all dependencies
        //return from item in DataSource.Dependencies
        //       select item;
    }

    public void Update(Dependency item)
    {
        //int id = item.Id;
        //const string dependenciesFile = @"..\xml\dependencies.xml";
        ////XElement dependenciesElement = XElement.Load(dependenciesFile); // למה אין כאן .ROOT והאם צריך ?
        //XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;

        //if (allDependencies!.Elements("Dependency").Any(dependency => (int)dependency.Element("Id")! == id)) //האם אפשר לשים !
        //    throw new DalAlreadyExistsException($"Dependency with ID={id} already exists");

        //XElement newDependencyElement = new XElement("Dependency",
        //    new XElement("Id", id),
        //    new XElement("DependentTask", item.DependentTask),
        //    new XElement("DependsOnTask", item.DependsOnTask)
        //    );
        //allDependencies.Add(newDependencyElement);
        //allDependencies.Save(dependenciesFile);
        //return id;
    }
}

