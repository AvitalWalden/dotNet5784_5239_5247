namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;

internal class Engineerlementation : IEngineer
{
    public int Create(Engineer item)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        lst.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(lst, "engineers");
        return item.Id;
    }

    public void Delete(int id)
    {
        //List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        //lst.Remove(Read(id)!);
        //XMLTools.SaveListToXMLSerializer<Engineer>(lst, "engineers");
        throw new DalDeletionImpossible($"Engineer with ID={id} cannot be deleted");
    }

    public Engineer? Read(int id)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        return lst.FirstOrDefault(engineer => engineer?.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (filter != null)
        {
            return lst.Where(filter).FirstOrDefault();
        }
        return lst.FirstOrDefault(engineer => filter!(engineer!));
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (filter != null)
        {
            return lst.Where(filter);
        }
        return lst;
    }

    public void Update(Engineer item)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        Engineer? engineer = lst.FirstOrDefault(engineer => engineer?.Id == item.Id);
        if (engineer is null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} is not exists");
        lst.Remove(engineer);
        lst.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(lst, "engineers");
    }

    public void Reset()
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        lst.Clear();
        XMLTools.SaveListToXMLSerializer<Engineer>(lst, "engineers");
    }
}
