namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// Method to create a new dependency
    /// </summary>
    /// <param name="item">dependency details to add</param>
    /// <returns>the new id of dependency</returns>
    /// <exception cref="DalAlreadyExistsException">If the dependency already exists</exception>
    public int Create(Dependency item)
    {
        int id = XMLTools.GetAndIncreaseNextId("data-config", "NextDependencyId");
        const string dependenciesFile = @"..\xml\dependencies.xml";
        XElement? allDependencies = XDocument.Load(dependenciesFile).Root;

        if (allDependencies!.Elements("Dependency").Any(dependency => (int)dependency.Element("Id")! == id)) 
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

    /// <summary>
    ///  Method to delete an dependency by ID
    /// </summary>
    /// <param name="id">ID of dependency you want to delete</param>
    /// <exception cref="DalDoesNotExistException">exeption that dependency cannot be deleted</exception>
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

    /// <summary>
    /// Method to read an dependency using a custom filter
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>Returns Dependency according to ID and according to the existence of the filter</returns>
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

    /// <summary>
    ///   Method to read an dependency by ID
    /// </summary>
    /// <param name="id">ID of dependency you want to read</param>
    /// <returns>returns dependency by ID</returns>
    public Dependency? Read(int id)
    {
        XElement? allDependencies = XDocument.Load(@"..\xml\dependencies.xml").Root;

        XElement? dependencyElement = allDependencies?
                    .Elements("Dependency")
                    .FirstOrDefault(dependency => (int)dependency?.Element("Id")! == id); 

        if (dependencyElement != null)
        {
            Dependency? dependency = new Dependency((int)dependencyElement.Element("Id")!, (int)dependencyElement.Element("DependentTask")!, (int)dependencyElement.Element("DependsOnTask")!);
            return dependency;
        }
        return null;
    }

    /// <summary>
    /// Method to read all Dependencies with an optional filter
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>read all dependency, or read all dependency that remain after the filter function</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement? dependenciesElement = XMLTools.LoadListFromXMLElement("dependencies");

        IEnumerable<Dependency> dependencies = dependenciesElement
            .Elements("Dependency")
            .Select(e => new Dependency(
                Id: (int)e.Element("Id")!,
                DependentTask: (int)e.Element("DependentTask")!,
                DependsOnTask: (int)e.Element("DependsOnTask")!
            ));

        if (filter != null)
        {
            dependencies = dependencies.Where(filter);
        }

        return dependencies;
    }
    /// <summary>
    ///  Method to update an existing dependency
    /// </summary>
    /// <param name="item">Dependency details to update</param>
    /// <exception cref="DalDoesNotExistException">If the dependency does not exists</exception>
    public void Update(Dependency item)
    {
        int id = item.Id;
        // call to XML file
        const string dependenciesFile = @"..\xml\dependencies.xml";
        XElement? allDependencies = XDocument.Load(dependenciesFile).Root;
        // Check if there is an engineer with the same ID
        XElement? dependencyToUpdate = allDependencies!.Descendants("Dependency").FirstOrDefault(dependency => (int)dependency.Element("Id")! == id);
        if (dependencyToUpdate != null)
        {
            // Update the task in the XML file
            dependencyToUpdate.ReplaceWith(
                new XElement("Dependency",
                new XElement("Id", id),
                new XElement("DependentTask", item.DependentTask),
                new XElement("DependsOnTask", item.DependsOnTask)
                )
            );
            // Save to XML file again
            allDependencies.Save(dependenciesFile);
        }
        else
        {
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");
        }
    }

    /// <summary>
    ///  Method to reset the list of Dependency
    /// </summary>
    public void Reset()
    {
        List<Dependency> lst = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencies");
        if (lst.Any())
        {
            lst.Clear();
            XMLTools.SaveListToXMLSerializer<Dependency>(lst, "dependencies");
        }
        string configFile = "data-config";
        XElement configElement = XMLTools.LoadListFromXMLElement(configFile);
        configElement.Element("NextDependencyId")?.SetValue("1");
        XMLTools.SaveListToXMLElement(configElement, configFile);
    }
}

