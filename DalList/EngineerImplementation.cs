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
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    // This method is used to delete an Engineer by ID
    public void Delete(int id)
    {
        //Engineer? engneerToDelete = Read(id);
        //if (engneerToDelete is not null)
        //{
        //    engneerToDelete.Active = false;
        //}
        // Throw an exception if the engineer cannot be deleted
        throw new DalDeletionImpossible($"Engineer with ID={id} cannot be deleted");
    }

    //Reads engineer object by filter function
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(engineer => filter(engineer!));
    }

    // This method is used to read an Engineer by ID
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(engineer => engineer?.Id == id);
    }

    // This method is used to read all Engineers
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        // If no filter is provided, return all engineers
        return from item in DataSource.Engineers
               select item;
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
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");
        }
    }

    public void Reset()
    {
        DataSource.Engineers.Clear();
    }
}
