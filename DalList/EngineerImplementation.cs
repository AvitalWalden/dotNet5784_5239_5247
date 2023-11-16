namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    // This method is used to create a new Engineer
    public int Create(Engineer item)
    {
        // Check if an Engineer with the same ID already exists
        if (Read(item.Id) is not null)
            throw new Exception($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    // This method is used to delete an Engineer by ID
    public void Delete(int id)
    { 
        // Check if there are any Tasks associated with the Engineer
        if (DataSource.Tasks.Any(task => task?.EngineerId == id))
            throw new Exception($"Engineer with ID={id} cannot be deleted");
        Engineer? engneerToDelete = Read(id);
        if (engneerToDelete is not null)
        {
            DataSource.Engineers.Remove(engneerToDelete);

        }
        else
        {
            throw new Exception($"Engineer with ID={id} not exists");
        }
    }
    // This method is used to read an Engineer by ID
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(engineer => engineer?.Id == id);
    }

    // This method is used to read all Engineers
    public List<Engineer> ReadAll()
    {
        return DataSource.Engineers.ToList()!;
    }

    // This method is used to update the engineer 
    public void Update(Engineer item)
    {
        Engineer? engineerToUpdate = DataSource.Engineers.FirstOrDefault(engineer => engineer?.Id == item.Id);
        if (engineerToUpdate is not null)
        {
            DataSource.Engineers.Remove(engineerToUpdate);
            DataSource.Engineers.Add(item);
        }
        else
        {
            throw new Exception($"Engineer with ID={item.Id} does not exist");
        }
    }
}
