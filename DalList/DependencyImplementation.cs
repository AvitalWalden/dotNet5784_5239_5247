namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        //for entities with auto id
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        if (DataSource.Dependencies.Exists(dependency => dependency.Id == id))
        {
            Dependency? dependency = DataSource.Dependencies.Find(dependency => dependency.Id == id);
            if (dependency is not null)
            {
                return dependency;
            }
        }
        return null;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        if (DataSource.Dependencies.Exists(dependency => dependency.Id == item.Id))
        {
            Dependency? dependency = DataSource.Dependencies.Find(dependency => dependency.Id == item.Id);
            if (dependency is not null)
            {
                DataSource.Dependencies.Remove(dependency);
                DataSource.Dependencies.Add(item);
            }
        }
        else
        {
            throw new Exception($"Dependency with ID={item.Id} not exists");
        }
    }
}
