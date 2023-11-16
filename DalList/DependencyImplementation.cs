namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
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

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(dependency => filter(dependency!));
    }

    // This method is used to read an dependency by ID
    public Dependency? Read(int id)
    {   
        // return the dependency with the given ID or null
         return DataSource.Dependencies.FirstOrDefault(dependency => dependency?.Id == id);
    }

    // This method is used to read all dependency
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null) // האם לשים ? בהחזרה
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        // If no filter is provided, return all dependencies
        return from item in DataSource.Dependencies
               select item;
    }

    // This method is used to update a dependency
    public void Update(Dependency item)
    {
        // Find the dependency with the given ID
        Dependency? dependency = DataSource.Dependencies.FirstOrDefault(dependency => dependency?.Id == item.Id);
        if (dependency is not null)
        {            
            // If the dependency is not null, remove it from the list and add the updated item
            DataSource.Dependencies.Remove(dependency);
            DataSource.Dependencies.Add(item);
        }
        else
        {
            // Throw an exception if the dependency with the given ID does not exist
            throw new Exception($"Dependency with ID={item.Id} does not exist");
        }
    }
}
