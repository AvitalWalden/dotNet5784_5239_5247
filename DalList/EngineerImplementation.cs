namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
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
        for (int i = 0; i < DataSource.Tasks.Count; i++)
        {
            if (DataSource.Tasks[i].Engineerld == id) 
            { 
                throw new Exception($"Engineer with ID={id} cannot be deleted");
            }
        }
        if (Read(id) is not null)
        {
            DataSource.Tasks.RemoveAt(id);

        }
        else
        {
            throw new Exception($"Engineer with ID={id} not exists");
        }
    }
    // This method is used to read an Engineer by ID
    public Engineer? Read(int id)
    {
        if (DataSource.Engineers.Exists(engineer => engineer.Id == id))
        {
            Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == id);
            if (engineer is not null)
            {
                return engineer;

            }
        }
        return null;
    }

    // This method is used to read all Engineers
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    // This method is used to update the engineer 
    public void Update(Engineer item)
    {
        if(DataSource.Engineers.Exists(engineer => engineer.Id == item.Id))
        {
            Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == item.Id);
            if (engineer is not null)
            {
                DataSource.Engineers.Remove(engineer);
                DataSource.Engineers.Add(item);
            }

        }
        else
        {
            throw new Exception($"Engineer with ID={item.Id} not exists");
        }
    }
}
