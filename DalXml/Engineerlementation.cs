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
    /// <summary>
    /// Method to create a new Engineer
    /// </summary>
    /// <param name="item">Engineer details to add</param>
    /// <returns>the new id of task</returns>
    /// <exception cref="DalAlreadyExistsException">If the engineer already exists</exception>
    public int Create(Engineer item)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        lst.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(lst, "engineers");
        return item.Id;
    }
    /// <summary>
    /// Method to delete an Engineer by ID
    /// </summary>
    /// <param name="id">ID of engineer you want to delete</param>
    /// <exception cref="DalDeletionImpossible">exeption that engineer cannot be deleted </exception>
    public void Delete(int id)
    {
        throw new DalDeletionImpossible($"Engineer with ID={id} cannot be deleted");
    }

    /// <summary>
    ///  Method to read an engineer by ID
    /// </summary>
    /// <param name="id">ID of engineer you want to read</param>
    /// <returns>returns engineer by ID</returns>
    //
    public Engineer? Read(int id)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        return lst.FirstOrDefault(engineer => engineer?.Id == id);
    }

    /// <summary>
    /// Method to read an Engineer using a custom filter
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>Returns Engineer according to ID and according to the existence of the filter</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (filter != null)
        {
            return lst.Where(filter).FirstOrDefault();
        }
        return lst.FirstOrDefault(engineer => filter!(engineer!));
    }

    /// <summary>
    /// Method to read all Engineers with an optional filter
    /// </summary>
    /// <param name="filter">The filter function</param>
    /// <returns>read all engineer, or read all engineer that remain after the filter function</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (filter != null)
        {
            return lst.Where(filter);
        }
        return lst;
    }

    /// <summary>
    ///  Method to update an existing Engineer
    /// </summary>
    /// <param name="item">Engineer details to update</param>
    /// <exception cref="DalDoesNotExistException">If the engineer does not exists</exception>
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

    /// <summary>
    /// Method to reset the list of Engineers
    /// </summary>
    public void Reset()
    {
        List<Engineer> lst = XMLTools.LoadListFromXMLSerializer<Engineer>("engineers");
        if (lst.Any())
        {
            lst.Clear();
            XMLTools.SaveListToXMLSerializer<Engineer>(lst, "engineers");
        }
    }
}
