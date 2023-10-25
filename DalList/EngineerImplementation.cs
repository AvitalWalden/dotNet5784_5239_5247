namespace Dal;
using DalApi;
using DO;
using System.Collections;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        //for entities with normal id (not auto id)
        if (Read(item.Id) is not null)
            throw new Exception($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        for (int i = 0; i < DataSource.Tasks.Count; i++)
        {
            if(DataSource.Tasks[i].Engineerld == id) 
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

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

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
