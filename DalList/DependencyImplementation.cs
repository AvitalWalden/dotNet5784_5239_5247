namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    // This method is used to create a new dependency
    public int Create(Dependency item)
    {
        // For entities with auto id
        int id = DataSource.Config.NextDependencyId;
        // Create a copy of the item with the new ID
        Dependency copy = item with { Id = id };
        // Add the copy to the list of dependencies
        DataSource.Dependencies.Add(copy);
        // Return the new ID
        return id;
    }

    // This method is used to delete a dependency
    public void Delete(int id)
    {   
        // Throw an exception if the dependency with the given ID cannot be deleted
        throw new Exception($"Dependency with ID={id} cannot be deleted");
    }

    // This method is used to read an dependency by ID
    public Dependency? Read(int id)
    {   
        // Check if a dependency with the given ID exists
        if (DataSource.Dependencies.Exists(dependency => dependency.Id == id))
        {
            // Find the dependency with the given ID
            Dependency? dependency = DataSource.Dependencies.Find(dependency => dependency.Id == id);
            // If the dependency is not null, return it
            if (dependency is not null)
            {
                return dependency;
            }
        }
        // Return null if no dependency with the given ID is found
        return null;
    }

    // This method is used to read all dependency
    public List<Dependency> ReadAll()
    {
        // Return a new list containing all dependencies in the DataSource
        return new List<Dependency>(DataSource.Dependencies);
    }

    // This method is used to update a dependency
    public void Update(Dependency item)
    {
        // Check if a dependency with the given ID exists
        if (DataSource.Dependencies.Exists(dependency => dependency.Id == item.Id))
        {
            // Find the dependency with the given ID
            Dependency? dependency = DataSource.Dependencies.Find(dependency => dependency.Id == item.Id);
            // If the dependency is not null, remove it from the list and add the updated item
            if (dependency is not null)
            {
                DataSource.Dependencies.Remove(dependency);
                DataSource.Dependencies.Add(item);
            }
        }
        else
        {
            // Throw an exception if the dependency with the given ID does not exist
            throw new Exception($"Dependency with ID={item.Id} not exists");
        }
    }
}
